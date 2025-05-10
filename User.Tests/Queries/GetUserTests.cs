using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Enums;
using PTManagementSystem.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace User.Tests.Queries
{
    public class GetUserTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUser _getUser;

        public GetUserTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _getUser = new GetUser(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidUserId_ShouldReturnUser()
        {
        
            var userId = "123e4567-e89b-12d3-a456-426614174000"; // Valid Guid string
            var expectedUser = new UserDto
            {
                Id = Guid.Parse(userId),
                Email = "test@example.com",
                UserType = UserType.CLIENT
            };

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            var result = await _getUser.ExecuteAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(Guid.Parse(userId), result.Id);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.UserType, result.UserType);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            string? userId = null;

            await Assert.ThrowsAsync<ArgumentException>(() => _getUser.ExecuteAsync(userId!));
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            var userId = string.Empty;

            await Assert.ThrowsAsync<ArgumentException>(() => _getUser.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithWhitespaceUserId_ShouldThrowArgumentException()
        {
            var userId = "   ";

            await Assert.ThrowsAsync<ArgumentException>(() => _getUser.ExecuteAsync(userId));
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserNotFound_ShouldReturnNull()
        {
            var userId = "non-existent-user-id";

            _userRepositoryMock.Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync((UserDto?)null);

            var result = await _getUser.ExecuteAsync(userId);

            Assert.Null(result);
            _userRepositoryMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }
    }
}