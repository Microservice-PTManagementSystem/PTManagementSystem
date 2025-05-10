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

namespace User.Tests.Commands
{
    public class UpdateUserProfileTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserValidationService> _validationServiceMock;
        private readonly UpdateUserProfile _updateUserProfile;

        public UpdateUserProfileTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IUserValidationService>();
            _updateUserProfile = new UpdateUserProfile(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldUpdateUserProfile()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var userProfileDto = new UserProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Weight Loss, Muscle Gain",
                MedicalConditions = "None"
            };
            var userProfileEntity = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Weight Loss, Muscle Gain",
                MedicalConditions = "None"
            };

            _mapperMock.Setup(x => x.Map<UserProfile>(userProfileDto))
                .Returns(userProfileEntity);

            _validationServiceMock.Setup(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdateUserProfileAsync(userId, userProfileEntity))
                .Returns(Task.CompletedTask);

            
            await _updateUserProfile.ExecuteAsync(userId, userProfileDto);

            
            _mapperMock.Verify(x => x.Map<UserProfile>(userProfileDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(userId, userProfileEntity), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidData_ShouldThrowArgumentException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var userProfileDto = new UserProfileDto
            {
                FirstName = "", // Invalid: empty
                LastName = "", // Invalid: empty
                DateOfBirth = DateTime.MinValue, // Invalid: too old
                Gender = "", // Invalid: empty
                Height = -1.0, // Invalid: negative
                Weight = -1.0, // Invalid: negative
                FitnessGoals = "", // Invalid: empty
                MedicalConditions = "" // Invalid: empty
            };
            var userProfileEntity = new UserProfile
            {
                FirstName = "",
                LastName = "",
                DateOfBirth = DateTime.MinValue,
                Gender = "",
                Height = -1.0,
                Weight = -1.0,
                FitnessGoals = "",
                MedicalConditions = ""
            };

            _mapperMock.Setup(x => x.Map<UserProfile>(userProfileDto))
                .Returns(userProfileEntity);

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("FirstName", "First name is required"),
                new ValidationFailure("LastName", "Last name is required"),
                new ValidationFailure("DateOfBirth", "Invalid date of birth"),
                new ValidationFailure("Gender", "Gender is required"),
                new ValidationFailure("Height", "Height must be positive"),
                new ValidationFailure("Weight", "Weight must be positive"),
                new ValidationFailure("FitnessGoals", "Fitness goals cannot exceed 500 characters"),
                new ValidationFailure("MedicalConditions", "Medical conditions cannot exceed 500 characters")
            });

            _validationServiceMock.Setup(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()))
                .ReturnsAsync(validationResult);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateUserProfile.ExecuteAsync(userId, userProfileDto));
            
            Assert.Contains("Invalid user profile", exception.Message);
            Assert.Contains("First name is required", exception.Message);
            Assert.Contains("Last name is required", exception.Message);
            Assert.Contains("Invalid date of birth", exception.Message);
            Assert.Contains("Gender is required", exception.Message);
            Assert.Contains("Height must be positive", exception.Message);
            Assert.Contains("Weight must be positive", exception.Message);
            Assert.Contains("Fitness goals cannot exceed 500 characters", exception.Message);
            Assert.Contains("Medical conditions cannot exceed 500 characters", exception.Message);

            _mapperMock.Verify(x => x.Map<UserProfile>(userProfileDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(It.IsAny<string>(), It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;
            var userProfileDto = new UserProfileDto();

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateUserProfile.ExecuteAsync(userId!, userProfileDto));
            
            _mapperMock.Verify(x => x.Map<UserProfile>(It.IsAny<UserProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(It.IsAny<string>(), It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;
            var userProfileDto = new UserProfileDto();

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateUserProfile.ExecuteAsync(userId, userProfileDto));
            
            _mapperMock.Verify(x => x.Map<UserProfile>(It.IsAny<UserProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(It.IsAny<string>(), It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserProfile_ShouldThrowArgumentNullException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            UserProfileDto? userProfileDto = null;

            
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _updateUserProfile.ExecuteAsync(userId, userProfileDto!));
            
            _mapperMock.Verify(x => x.Map<UserProfile>(It.IsAny<UserProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(It.IsAny<string>(), It.IsAny<UserProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var userProfileDto = new UserProfileDto
            {
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Test",
                MedicalConditions = "None"
            };
            var userProfileEntity = new UserProfile
            {
                FirstName = "Test",
                LastName = "User",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Male",
                Height = 180.0,
                Weight = 80.0,
                FitnessGoals = "Test",
                MedicalConditions = "None"
            };
            var expectedError = new Exception("Database error");

            _mapperMock.Setup(x => x.Map<UserProfile>(userProfileDto))
                .Returns(userProfileEntity);

            _validationServiceMock.Setup(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdateUserProfileAsync(userId, userProfileEntity))
                .ThrowsAsync(expectedError);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _updateUserProfile.ExecuteAsync(userId, userProfileDto));
            
            Assert.Same(expectedError, exception);
            _mapperMock.Verify(x => x.Map<UserProfile>(userProfileDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateUserProfileAsync(It.IsAny<UserProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateUserProfileAsync(userId, userProfileEntity), Times.Once);
        }
    }
}