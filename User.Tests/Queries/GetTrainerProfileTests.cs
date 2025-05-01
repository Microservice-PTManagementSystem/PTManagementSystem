using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace User.Tests.Queries
{
    public class GetTrainerProfileTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTrainerProfile _getTrainerProfile;

        public GetTrainerProfileTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _getTrainerProfile = new GetTrainerProfile(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidUserId_ShouldReturnTrainerProfile()
        {
            
            var userId = "test-user-id";
            var expectedProfile = new TrainerProfileDto
            {
                Specialization = "Fitness Training",
                ExperienceYears = 5,
                Certifications = new List<CertificationDto>(),
                Bio = "Experienced trainer",
                HourlyRate = 50.0M,
                AvailableDays = new List<string> { "Monday", "Wednesday", "Friday" },
                AvailableHours = new List<TimeSpan> { TimeSpan.FromHours(9), TimeSpan.FromHours(10) }
            };

            _userRepositoryMock.Setup(x => x.GetTrainerProfileAsync(userId))
                .ReturnsAsync(expectedProfile);

            
            var result = await _getTrainerProfile.ExecuteAsync(userId);

            
            Assert.NotNull(result);
            Assert.Equal(expectedProfile.Specialization, result.Specialization);
            Assert.Equal(expectedProfile.ExperienceYears, result.ExperienceYears);
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getTrainerProfile.ExecuteAsync(userId!));
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getTrainerProfile.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithWhitespaceUserId_ShouldThrowArgumentException()
        {
            
            var userId = "   ";

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getTrainerProfile.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenProfileNotFound_ShouldReturnNull()
        {
            
            var userId = "non-existent-user-id";

            _userRepositoryMock.Setup(x => x.GetTrainerProfileAsync(userId))
                .ReturnsAsync((TrainerProfileDto?)null);

            
            var result = await _getTrainerProfile.ExecuteAsync(userId);

            
            Assert.Null(result);
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            
            var userId = "test-user-id";
            var expectedError = new Exception("Database error");

            _userRepositoryMock.Setup(x => x.GetTrainerProfileAsync(userId))
                .ThrowsAsync(expectedError);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _getTrainerProfile.ExecuteAsync(userId));
            
            Assert.Same(expectedError, exception);
            _userRepositoryMock.Verify(x => x.GetTrainerProfileAsync(userId), Times.Once);
        }
    }
}