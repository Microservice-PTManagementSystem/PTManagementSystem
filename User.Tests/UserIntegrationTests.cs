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
using System.Collections.Generic;
using User.Tests.Infrastructure.ExternalServices;

namespace PTManagementSystem.Tests
{
    public class UserIntegrationTests : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly UserDbContext _dbContext;
        private readonly Mock<IKeycloakService> _mockKeycloakService;
        private readonly MockRabbitMQService _mockRabbitMQService;

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
            _mockRabbitMQService = new MockRabbitMQService();
            services.AddSingleton<IMessageBroker>(_mockRabbitMQService);
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
                KeycloakUserId = "11111111-1111-1111-1111-111111111111"
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
                KeycloakUserId = "22222222-2222-2222-2222-222222222222"
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
                KeycloakUserId = "33333333-3333-3333-3333-333333333333"
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
                KeycloakUserId = "44444444-4444-4444-4444-444444444444"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();

            // initial user profile
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
            result!.FirstName.Should().Be(userProfileDto.FirstName);
            result.LastName.Should().Be(userProfileDto.LastName);
        }

        [Fact]
        public async Task UpdatePaymentInfo_ShouldUpdateUserPaymentDetails()
        {
            
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var updatePaymentInfo = _serviceProvider.GetRequiredService<UpdatePaymentInfo>();

            var registerCommand = new KeycloakRegisterDto
            {
                KeycloakUserId = "55555555-5555-5555-5555-555555555555"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();

            var paymentInfoDto = new PaymentInfoDto
            {
                CardNumber = "4111111111111111",
                CardHolderName = "Test User",
                ExpiryDate = "12/25",
                CVV = "123",
                BillingAddress = "123 Payment Street, Payment City, Payment State, 54321"
            };

            
            await updatePaymentInfo.ExecuteAsync(user.KeycloakId, paymentInfoDto);

            
            var updatedUser = await _dbContext.Users.Find(u => u.KeycloakId == user.KeycloakId).FirstOrDefaultAsync();
            updatedUser.Should().NotBeNull();
            updatedUser.PaymentInfo.Should().NotBeNull();
            updatedUser.PaymentInfo.CardNumber.Should().Be(paymentInfoDto.CardNumber);
            updatedUser.PaymentInfo.CardHolderName.Should().Be(paymentInfoDto.CardHolderName);
        }

        [Fact]
        public async Task UpdateTrainerProfile_ShouldUpdateTrainerDetails()
        {
           
            var registerUser = _serviceProvider.GetRequiredService<RegisterUser>();
            var updateTrainerProfile = _serviceProvider.GetRequiredService<UpdateTrainerProfile>();

            var registerCommand = new KeycloakRegisterDto
            {
                KeycloakUserId = "66666666-6666-6666-6666-666666666666"
            };

            await registerUser.RegisterAsync(registerCommand);

            var user = await _dbContext.Users.Find(u => u.KeycloakId == registerCommand.KeycloakUserId).FirstOrDefaultAsync();
            user.Should().NotBeNull();

            var trainerProfileDto = new TrainerProfileDto
            {
                Specialization = "Fitness Training",
                ExperienceYears = 5,
                Certifications = new List<CertificationDto>
                {
                    new CertificationDto
                    {
                        Name = "NASM",
                        IssuingAuthority = "National Academy of Sports Medicine",
                        IssueDate = DateTime.Now.AddYears(-1),
                        ExpiryDate = DateTime.Now.AddYears(1)
                    }
                },
                Bio = "Experienced fitness trainer",
                HourlyRate = 50.0m,
                AvailableDays = new List<string> { "Monday", "Wednesday", "Friday" },
                AvailableHours = new List<TimeSpan> { new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0) }
            };

            
            await updateTrainerProfile.ExecuteAsync(user.KeycloakId, trainerProfileDto);

            
            var updatedUser = await _dbContext.Users.Find(u => u.KeycloakId == user.KeycloakId).FirstOrDefaultAsync();
            updatedUser.Should().NotBeNull();
            updatedUser.TrainerProfile.Should().NotBeNull();
            updatedUser.TrainerProfile!.Specialization.Should().Be(trainerProfileDto.Specialization);
            updatedUser.TrainerProfile.ExperienceYears.Should().Be(trainerProfileDto.ExperienceYears);
            updatedUser.TrainerProfile.Certifications.Should().HaveCount(trainerProfileDto.Certifications.Count);
        }

    

        public void Dispose()
        {
            // Clean up test database
            _dbContext.Users.DeleteMany(Builders<Domain.Entities.User>.Filter.Empty);
            _serviceProvider.Dispose();
        }
    }
}