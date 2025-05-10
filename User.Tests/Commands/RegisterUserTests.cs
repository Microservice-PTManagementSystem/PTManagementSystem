using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Commands;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using PTManagementSystem.Domain.Enums;

namespace User.Tests.Commands
{
    public class RegisterUserTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IKeycloakService> _keycloakServiceMock;
        private readonly Mock<IUserValidationService> _validationServiceMock;
        private readonly RegisterUser _registerUser;

        public RegisterUserTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _keycloakServiceMock = new Mock<IKeycloakService>(MockBehavior.Strict);
            _validationServiceMock = new Mock<IUserValidationService>(MockBehavior.Strict);
            _registerUser = new RegisterUser(
                _keycloakServiceMock.Object,
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldRegisterUser()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            var keycloakUser = new KeycloakUserDto
            {
                Email = "test@example.com",
                EmailVerified = true
            };

            var roles = new List<string> { "Client" };

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(keycloakUser);

            _keycloakServiceMock.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            _validationServiceMock.Setup(x => x.ValidateUserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(true);

            
            await _registerUser.RegisterAsync(keycloakRegisterDto);

            
            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Once);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WhenKeycloakUserNotFound_ShouldThrowException()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync((KeycloakUserDto?)null);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _registerUser.RegisterAsync(keycloakRegisterDto));
            
            Assert.Equal("Keycloak user not found.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenNoRolesFound_ShouldThrowException()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            var keycloakUser = new KeycloakUserDto
            {
                Email = "test@example.com",
                EmailVerified = true
            };

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(keycloakUser);

            _keycloakServiceMock.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<string>());

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _registerUser.RegisterAsync(keycloakRegisterDto));
            
            Assert.Equal("No roles found for this user.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenInvalidRole_ShouldThrowException()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            var keycloakUser = new KeycloakUserDto
            {
                Email = "test@example.com",
                EmailVerified = true
            };

            var roles = new List<string> { "InvalidRole" };

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(keycloakUser);

            _keycloakServiceMock.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _registerUser.RegisterAsync(keycloakRegisterDto));
            
            Assert.Equal("Invalid user role.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Never);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenValidationFails_ShouldThrowArgumentException()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            var keycloakUser = new KeycloakUserDto
            {
                Email = "test@example.com",
                EmailVerified = true
            };

            var roles = new List<string> { "Client" };

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("KeycloakId", "User already exists")
            });

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(keycloakUser);

            _keycloakServiceMock.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            _validationServiceMock.Setup(x => x.ValidateUserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(validationResult);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _registerUser.RegisterAsync(keycloakRegisterDto));
            
            Assert.Contains("Invalid user data", exception.Message);
            Assert.Contains("User already exists", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Once);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WhenRepositoryFails_ShouldThrowException()
        {
            
            var keycloakRegisterDto = new KeycloakRegisterDto
            {
                KeycloakUserId = "test-user-id"
            };

            var keycloakUser = new KeycloakUserDto
            {
                Email = "test@example.com",
                EmailVerified = true
            };

            var roles = new List<string> { "Client" };

            _keycloakServiceMock.Setup(x => x.GetUserInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(keycloakUser);

            _keycloakServiceMock.Setup(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()))
                .ReturnsAsync(roles);

            _validationServiceMock.Setup(x => x.ValidateUserExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()))
                .ReturnsAsync(false);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _registerUser.RegisterAsync(keycloakRegisterDto));
            
            Assert.Equal("User could not be registered.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserInfoAsync(It.IsAny<string>()), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesIncludingGroupsAsync(It.IsAny<string>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserExistsAsync(It.IsAny<string>()), Times.Once);
            _userRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<RegisterUserDto>()), Times.Once);
        }
    }
} 