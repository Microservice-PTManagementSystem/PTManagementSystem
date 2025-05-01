using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Commands;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Enums;
using PTManagementSystem.Domain.EventModels;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;
using System.Threading.Tasks;
using System;
using System.Linq;
using PTManagementSystem.Domain.Entities;

namespace User.Tests.Commands
{
    public class DeleteUserTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IKeycloakService> _keycloakServiceMock;
        private readonly Mock<IMessageBroker> _messageBrokerMock;
        private readonly Mock<IUserValidationService> _validationServiceMock;
        private readonly DeleteUser _deleteUser;

        public DeleteUserTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _keycloakServiceMock = new Mock<IKeycloakService>();
            _messageBrokerMock = new Mock<IMessageBroker>();
            _validationServiceMock = new Mock<IUserValidationService>();
            _deleteUser = new DeleteUser(
                _keycloakServiceMock.Object,
                _messageBrokerMock.Object,
                _validationServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldDeleteUser()
        {
            
            var email = "test@example.com";
            var userId = "12345678-1234-1234-1234-123456789012";
            var roles = new[] { "Client" };

            _keycloakServiceMock.Setup(x => x.GetUserIdByEmailAsync(email))
                .ReturnsAsync(userId);
            _validationServiceMock.Setup(x => x.ValidateDeleteUserAsync(userId))
                .ReturnsAsync(new ValidationResult());
            _keycloakServiceMock.Setup(x => x.GetUserRolesAsync(userId))
                .ReturnsAsync(roles);
            _keycloakServiceMock.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(true);

            
            var result = await _deleteUser.ExecuteAsync(email);

            
            Assert.True(result);
            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(email), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(userId), Times.Once);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithTrainerUser_ShouldDeleteUserAndPublishEvent()
        {
            
            var email = "trainer@example.com";
            var userId = "12345678-1234-1234-1234-123456789012";
            var roles = new[] { "Trainer" };

            _keycloakServiceMock.Setup(x => x.GetUserIdByEmailAsync(email))
                .ReturnsAsync(userId);
            _validationServiceMock.Setup(x => x.ValidateDeleteUserAsync(userId))
                .ReturnsAsync(new ValidationResult());
            _keycloakServiceMock.Setup(x => x.GetUserRolesAsync(userId))
                .ReturnsAsync(roles);
            _keycloakServiceMock.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(true);
            _messageBrokerMock.Setup(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()))
                .Returns(Task.CompletedTask);

           
            var result = await _deleteUser.ExecuteAsync(email);

            
            Assert.True(result);
            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(email), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(userId), Times.Once);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.Is<TrainerDeletedEvent>(e => 
                e.TrainerId == Guid.Parse(userId))), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidUserId_ShouldThrowArgumentException()
        {
            
            var email = "test@example.com";
            var userId = "12345678-1234-1234-1234-123456789012";

            _keycloakServiceMock.Setup(x => x.GetUserIdByEmailAsync(email))
                .ReturnsAsync(userId);

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("UserId", "User ID is invalid"),
                new ValidationFailure("UserId", "User does not exist")
            });

            _validationServiceMock.Setup(x => x.ValidateDeleteUserAsync(userId))
                .ReturnsAsync(validationResult);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _deleteUser.ExecuteAsync(email));
            
            Assert.Contains("Invalid user ID", exception.Message);
            Assert.Contains("User ID is invalid", exception.Message);
            Assert.Contains("User does not exist", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(email), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(It.IsAny<string>()), Times.Never);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullEmail_ShouldThrowArgumentException()
        {
            
            string? email = null;

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _deleteUser.ExecuteAsync(email!));
            
            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(It.IsAny<string>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(It.IsAny<string>()), Times.Never);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyEmail_ShouldThrowArgumentException()
        {
            
            var email = string.Empty;

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _deleteUser.ExecuteAsync(email));
            
            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(It.IsAny<string>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(It.IsAny<string>()), Times.Never);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserNotFound_ShouldThrowArgumentException()
        {
            
            var email = "nonexistent@example.com";

            _keycloakServiceMock.Setup(x => x.GetUserIdByEmailAsync(email))
                .ReturnsAsync((string?)null);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _deleteUser.ExecuteAsync(email));
            
            Assert.Equal("A user with the specified email was not found.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(email), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(It.IsAny<string>()), Times.Never);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(It.IsAny<string>()), Times.Never);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenKeycloakDeleteFails_ShouldThrowApplicationException()
        {
            
            var email = "test@example.com";
            var userId = "12345678-1234-1234-1234-123456789012";
            var roles = new[] { "Client" };

            _keycloakServiceMock.Setup(x => x.GetUserIdByEmailAsync(email))
                .ReturnsAsync(userId);
            _validationServiceMock.Setup(x => x.ValidateDeleteUserAsync(userId))
                .ReturnsAsync(new ValidationResult());
            _keycloakServiceMock.Setup(x => x.GetUserRolesAsync(userId))
                .ReturnsAsync(roles);
            _keycloakServiceMock.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(false);

            
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => 
                _deleteUser.ExecuteAsync(email));
            
            Assert.Equal("User could not be deleted. Negative response from Keycloak.", exception.Message);

            _keycloakServiceMock.Verify(x => x.GetUserIdByEmailAsync(email), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateDeleteUserAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.GetUserRolesAsync(userId), Times.Once);
            _keycloakServiceMock.Verify(x => x.DeleteUserAsync(userId), Times.Once);
            _messageBrokerMock.Verify(x => x.PublishAsync(It.IsAny<TrainerDeletedEvent>()), Times.Never);
        }
    }
} 