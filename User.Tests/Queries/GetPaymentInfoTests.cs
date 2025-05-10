using Moq;
using Xunit;
using PTManagementSystem.Application.UseCases.Queries;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Entities;
using System.Threading.Tasks;
using System;
using AutoMapper;

namespace User.Tests.Queries
{
    public class GetPaymentInfoTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPaymentInfo _getPaymentInfo;

        public GetPaymentInfoTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _getPaymentInfo = new GetPaymentInfo(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidUserId_ShouldReturnPaymentInfo()
        {
            
            var userId = "test-user-id";
            var paymentInfo = new PaymentInfo
            {
                CardHolderName = "John Doe",
                CardNumber = "1234567890123456",
                ExpiryDate = "12/25",
                CVV = "123",
                BillingAddress = "123 Test St"
            };
            var paymentInfoDto = new PaymentInfoDto
            {
                CardHolderName = "John Doe",
                CardNumber = "1234567890123456",
                ExpiryDate = "12/25",
                CVV = "123",
                BillingAddress = "123 Test St"
            };

            _userRepositoryMock.Setup(x => x.GetPaymentInfoAsync(userId))
                .ReturnsAsync(paymentInfoDto);

            
            var result = await _getPaymentInfo.ExecuteAsync(userId);

            
            Assert.NotNull(result);
            Assert.Equal(paymentInfoDto.CardNumber, result.CardNumber);
            Assert.Equal(paymentInfoDto.CardHolderName, result.CardHolderName);
            Assert.Equal(paymentInfoDto.ExpiryDate, result.ExpiryDate);
            Assert.Equal(paymentInfoDto.CVV, result.CVV);
            Assert.Equal(paymentInfoDto.BillingAddress, result.BillingAddress);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getPaymentInfo.ExecuteAsync(userId!));
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;

            
            await Assert.ThrowsAsync<ArgumentException>(() => _getPaymentInfo.ExecuteAsync(userId));
        }

        [Fact]
        public async Task ExecuteAsync_WhenUserNotFound_ShouldReturnNull()
        {
            
            var userId = "non-existent-user";
            _userRepositoryMock.Setup(x => x.GetPaymentInfoAsync(userId))
                .ReturnsAsync((PaymentInfoDto?)null);

            
            var result = await _getPaymentInfo.ExecuteAsync(userId);

            
            Assert.Null(result);
        }
    }
}