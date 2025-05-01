using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Application.Services;
using PTManagementSystem.Application.UseCases.Commands;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Domain.Enums;
using PTManagementSystem.Infrastructure;
using PTManagementSystem.Infrastructure.Data;
using PTManagementSystem.Infrastructure.ExternalServices;
using PTManagementSystem.Infrastructure.Repositories;
using PTManagementSystem.Infrastructure.Config;
using PTManagementSystem.Presentation;
using PTManagementSystem.Presentation.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using Moq;

namespace PTManagementSystem.Tests
{
    public class UserIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly UserDbContext _dbContext;
        private readonly Mock<IKeycloakService> _mockKeycloakService;

        static UserIntegrationTests()
        {
            // Configure MongoDB Guid representation - only once for all tests
            try
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            }
            catch (BsonSerializationException)
            {
                // Serializer already registered
            }
        }

        public UserIntegrationTests()
        {
            var services = new ServiceCollection();
            
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddSingleton<IConfiguration>(configuration);

            // Configure MongoDB settings
            services.Configure<DatabaseSettings>(options =>
            {
                options.ConnectionString = "mongodb://localhost:27017";
                options.DatabaseName = "TestDatabase";
            });

            // mock KeycloakService
            _mockKeycloakService = new Mock<IKeycloakService>();
            _mockKeycloakService.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new KeycloakUserDto
                {
                    Email = "test@example.com",
                    EmailVerified = true
                });
            _mockKeycloakService.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(new[] { "Client" });
            _mockKeycloakService.Setup(x => x.GetAccessTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("test-access-token");

            // Register mock service
            services.AddSingleton(_mockKeycloakService.Object);

            // AutoMapper
            services.AddAutoMapper(typeof(PTManagementSystem.Application.Mappings.MappingProfile));

            //  UseCases
            services.AddScoped<RegisterUser>();
            services.AddScoped<LoginUser>();
            services.AddScoped<DeleteUser>();
            services.AddScoped<UpdatePaymentInfo>();
            services.AddScoped<UpdateTrainerProfile>();
            services.AddScoped<UpdateUserProfile>();
            services.AddScoped<GetPaymentInfo>();
            services.AddScoped<GetTrainerProfile>();
            services.AddScoped<GetUserProfile>();
            services.AddScoped<GetUser>();

            //  Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<IMessageBroker, RabbitMQService>();
            services.AddScoped<IUserValidationService, UserValidationService>();

            //  DbContext
            services.AddSingleton<UserDbContext>();

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<UserDbContext>();
        }

        [Fact]
        public async Task RegisterUser_ShouldCreateNewUser()
        {
            
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var command = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            
            await registerUser.RegisterAsync(command);

            
            var user = await _dbContext.Users.Find(u => u.KeycloakId == command.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();
            user.Email.Should().NotBeNullOrEmpty();
            user.UserType.Should().BeOneOf(UserType.CLIENT, UserType.TRAINER);
        }

        [Fact]
        public async Task LoginUser_WithValidCredentials_ShouldReturnToken()
        {
            
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var loginUser = _serviceProvider.GetRequiredService<LoginUser>();

            var registerCommand = new KeycloakRegisterDto
            {
                KeycloakUserId = "login-test-user-id"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            var loginDto = new LoginDto
            {
                Email = user.Email,
                Password = "Test123!"
            };

            
            var result = await loginUser.ExecuteAsync(loginDto);

            
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateUserProfile_ShouldUpdateUserDetails()
        {
            
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var updateProfile = _serviceProvider.GetRequiredService<UpdateUserProfile>();

            var registerCommand = new KeycloakRegisterDto
            {
                KeycloakUserId = "update-test-user-id"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();

            var userProfileDto = new UserProfileDto
            {
                FirstName = "Updated",
                LastName = "Name",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Test goals",
                MedicalConditions = "None",
                Allergies = "None",
                Address = new AddressDto
                {
                    Street = "Test Street",
                    City = "Test City",
                    State = "Test State",
                    Country = "Test Country",
                    PostalCode = "12345"
                }
            };

            
            await updateProfile.ExecuteAsync(user.KeycloakId, userProfileDto);

            
            var updatedUser = await _dbContext.Users.Find(u => u.KeycloakId == user.KeycloakId).FirstOrDefaultAsync();
            updatedUser.Should().NotBeNull();
            updatedUser.UserProfile.Should().NotBeNull();
            updatedUser.UserProfile.FirstName.Should().Be(userProfileDto.FirstName);
            updatedUser.UserProfile.LastName.Should().Be(userProfileDto.LastName);
            updatedUser.UserProfile.PhoneNumber.Should().Be(userProfileDto.PhoneNumber);
        }

        [Fact]
        public async Task GetUserProfile_ShouldReturnUserDetails()
        {
            
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var getUserProfile = _serviceProvider.GetRequiredService<GetUserProfile>();

            var registerCommand = new KeycloakRegisterDto
            {
                KeycloakUserId = "getprofile-test-user-id"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();

            // Create initial user profile
            var userProfileDto = new UserProfileDto
            {
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Test goals",
                MedicalConditions = "None",
                Allergies = "None",
                Address = new AddressDto
                {
                    Street = "Test Street",
                    City = "Test City",
                    State = "Test State",
                    Country = "Test Country",
                    PostalCode = "12345"
                }
            };

            // Update user profile 
            var updateProfile = _serviceProvider.GetRequiredService<UpdateUserProfile>();
            await updateProfile.ExecuteAsync(user.KeycloakId, userProfileDto);

            
            var result = await getUserProfile.ExecuteAsync(user.KeycloakId);

            
            result.Should().NotBeNull();
            result.FirstName.Should().Be(userProfileDto.FirstName);
            result.LastName.Should().Be(userProfileDto.LastName);
        }

        public void Dispose()
        {
            // Clean up test database
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase("TestDatabase");
            _serviceProvider.Dispose();
        }
    }
}