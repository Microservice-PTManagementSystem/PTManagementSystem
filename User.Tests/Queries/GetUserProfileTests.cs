using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Entities;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace User.Tests.Queries
{
    public class GetUserProfileTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserProfile _getUserProfile;

        public GetUserProfileTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _getUserProfile = new GetUserProfile(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidUserId_ShouldReturnUserProfile()
        {
            
            var userId = "test-user-id";
            var expectedProfile = new UserProfileDto
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890",
                Address = new AddressDto
                {
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001"
                }
            };

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(userId))
                .ReturnsAsync(expectedProfile);

            
            var result = await _getUserProfile.ExecuteAsync(userId);

            
            Assert.NotNull(result);
            Assert.Equal(expectedProfile.FirstName, result.FirstName);
            Assert.Equal(expectedProfile.LastName, result.LastName);
            Assert.Equal(expectedProfile.PhoneNumber, result.PhoneNumber);
            Assert.Equal(expectedProfile.Address.Street, result.Address.Street);
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getUserProfile.ExecuteAsync(userId!));
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getUserProfile.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithWhitespaceUserId_ShouldThrowArgumentException()
        {
            
            var userId = "   ";

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getUserProfile.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenProfileNotFound_ShouldThrowKeyNotFoundException()
        {
            
            var userId = "non-existent-user-id";

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(userId))
                .ReturnsAsync((UserProfileDto?)null);

            
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                _getUserProfile.ExecuteAsync(userId));
            
            Assert.Equal($"User profile not found for user ID: {userId}", exception.Message);
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            
            var userId = "test-user-id";
            var expectedError = new Exception("Database error");

            _userRepositoryMock.Setup(x => x.GetUserProfileAsync(userId))
                .ThrowsAsync(expectedError);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _getUserProfile.ExecuteAsync(userId));
            
            Assert.Same(expectedError, exception);
            _userRepositoryMock.Verify(x => x.GetUserProfileAsync(userId), Times.Once);
        }
    }
}