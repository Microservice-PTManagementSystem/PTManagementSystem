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
    public class UpdateTrainerProfileTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserValidationService> _validationServiceMock;
        private readonly UpdateTrainerProfile _updateTrainerProfile;

        public UpdateTrainerProfileTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _validationServiceMock = new Mock<IUserValidationService>(MockBehavior.Strict);
            _updateTrainerProfile = new UpdateTrainerProfile(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldUpdateTrainerProfile()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var trainerProfileDto = new TrainerProfileDto
            {
                Specialization = "Fitness Training",
                ExperienceYears = 5,
                Certifications = new List<CertificationDto>
                {
                    new CertificationDto { Name = "NASM" },
                    new CertificationDto { Name = "ACE" }
                },
                HourlyRate = 50.0m
            };
            var trainerProfileEntity = new TrainerProfile
            {
                Specialization = "Fitness Training",
                ExperienceYears = 5,
                Certifications = new List<Certification>
                {
                    new Certification { Name = "NASM" },
                    new Certification { Name = "ACE" }
                },
                HourlyRate = 50.0m
            };

            _mapperMock.Setup(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()))
                .Returns(trainerProfileEntity);

            _validationServiceMock.Setup(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()))
                .Returns(Task.CompletedTask);

            
            await _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto);

            
            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidTrainerProfile_ShouldThrowArgumentException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var trainerProfileDto = new TrainerProfileDto
            {
                Specialization = "", // Invalid: empty specialization
                ExperienceYears = -1, // Invalid: negative experience
                Certifications = new List<CertificationDto>(), // Invalid: no certifications
                HourlyRate = -10.0m // Invalid: negative hourly rate
            };
            var trainerProfileEntity = new TrainerProfile
            {
                Specialization = "",
                ExperienceYears = -1,
                Certifications = new List<Certification>(),
                HourlyRate = -10.0m
            };

            _mapperMock.Setup(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()))
                .Returns(trainerProfileEntity);

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("Specialization", "Specialization is required"),
                new ValidationFailure("ExperienceYears", "Experience years must be positive"),
                new ValidationFailure("Certifications", "At least one certification is required"),
                new ValidationFailure("HourlyRate", "Hourly rate must be positive")
            });

            _validationServiceMock.Setup(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()))
                .ReturnsAsync(validationResult);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto));
            
            Assert.Contains("Invalid trainer profile", exception.Message);
            Assert.Contains("Specialization is required", exception.Message);
            Assert.Contains("Experience years must be positive", exception.Message);
            Assert.Contains("At least one certification is required", exception.Message);
            Assert.Contains("Hourly rate must be positive", exception.Message);

            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;
            var trainerProfileDto = new TrainerProfileDto();

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateTrainerProfile.ExecuteAsync(userId!, trainerProfileDto));
            
            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;
            var trainerProfileDto = new TrainerProfileDto();

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto));
            
            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullTrainerProfile_ShouldThrowArgumentNullException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            TrainerProfileDto? trainerProfileDto = null;

            
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto!));
            
            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            
            var userId = "12345678-1234-1234-1234-123456789012";
            var trainerProfileDto = new TrainerProfileDto
            {
                Specialization = "Test",
                ExperienceYears = 1,
                Certifications = new List<CertificationDto>(),
                HourlyRate = 10.0m
            };
            var trainerProfileEntity = new TrainerProfile
            {
                Specialization = "Test",
                ExperienceYears = 1,
                Certifications = new List<Certification>(),
                HourlyRate = 10.0m
            };
            var expectedError = new Exception("Database error");

            _mapperMock.Setup(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()))
                .Returns(trainerProfileEntity);

            _validationServiceMock.Setup(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()))
                .ThrowsAsync(expectedError);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto));
            
            Assert.Same(expectedError, exception);
            _mapperMock.Verify(x => x.Map<TrainerProfile>(It.IsAny<TrainerProfileDto>()), Times.Once);
            _validationServiceMock.Verify(x => x.ValidateTrainerProfileAsync(It.IsAny<TrainerProfile>()), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateTrainerProfileAsync(It.IsAny<string>(), It.IsAny<TrainerProfile>()), Times.Once);
        }
    }
} 