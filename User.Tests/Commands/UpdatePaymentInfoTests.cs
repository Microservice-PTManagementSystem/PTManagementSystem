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

namespace User.Tests.Commands
{
    public class UpdatePaymentInfoTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserValidationService> _validationServiceMock;
        private readonly UpdatePaymentInfo _updatePaymentInfo;

        public UpdatePaymentInfoTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IUserValidationService>();
            _updatePaymentInfo = new UpdatePaymentInfo(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidData_ShouldUpdatePaymentInfo()
        {
            
            var userId = "test-user-id";
            var paymentInfoDto = new PaymentInfoDto
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                ExpiryDate = "12/25",
                CVV = "123"
            };
            var paymentEntity = new PaymentInfo
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                ExpiryDate = "12/25",
                CVV = "123"
            };

            _mapperMock.Setup(x => x.Map<PaymentInfo>(paymentInfoDto))
                .Returns(paymentEntity);

            _validationServiceMock.Setup(x => x.ValidatePaymentInfoAsync(paymentEntity))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdatePaymentInfoAsync(userId, It.IsAny<PaymentInfo>()))
                .Returns(Task.CompletedTask);

           
            await _updatePaymentInfo.ExecuteAsync(userId, paymentInfoDto);

            
            _mapperMock.Verify(x => x.Map<PaymentInfo>(paymentInfoDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(paymentEntity), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(userId, paymentEntity), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidPaymentInfo_ShouldThrowArgumentException()
        {
            
            var userId = "test-user-id";
            var paymentInfoDto = new PaymentInfoDto
            {
                CardNumber = "1234", // Invalid: too short
                CardHolderName = "", // Invalid: empty
                ExpiryDate = "invalid", // Invalid: wrong format
                CVV = "1" // Invalid: too short
            };
            var paymentEntity = new PaymentInfo
            {
                CardNumber = "1234",
                CardHolderName = "",
                ExpiryDate = "invalid",
                CVV = "1"
            };

            _mapperMock.Setup(x => x.Map<PaymentInfo>(paymentInfoDto))
                .Returns(paymentEntity);

            var validationResult = new ValidationResult(new[]
            {
                new ValidationFailure("CardNumber", "Card number must be 16 digits"),
                new ValidationFailure("CardHolderName", "Card holder name is required"),
                new ValidationFailure("ExpiryDate", "Invalid expiry date format"),
                new ValidationFailure("CVV", "CVV must be 3 digits")
            });

            _validationServiceMock.Setup(x => x.ValidatePaymentInfoAsync(paymentEntity))
                .ReturnsAsync(validationResult);

            
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
                _updatePaymentInfo.ExecuteAsync(userId, paymentInfoDto));
            
            Assert.Contains("Invalid payment information", exception.Message);
            Assert.Contains("Card number must be 16 digits", exception.Message);
            Assert.Contains("Card holder name is required", exception.Message);
            Assert.Contains("Invalid expiry date format", exception.Message);
            Assert.Contains("CVV must be 3 digits", exception.Message);

            _mapperMock.Verify(x => x.Map<PaymentInfo>(paymentInfoDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(paymentEntity), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(
                It.IsAny<string>(), It.IsAny<PaymentInfo>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullUserId_ShouldThrowArgumentException()
        {
            
            string? userId = null;
            var paymentInfoDto = new PaymentInfoDto();

            
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updatePaymentInfo.ExecuteAsync(userId!, paymentInfoDto));
            
            _mapperMock.Verify(x => x.Map<PaymentInfo>(It.IsAny<PaymentInfoDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(It.IsAny<PaymentInfo>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(
                It.IsAny<string>(), It.IsAny<PaymentInfo>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyUserId_ShouldThrowArgumentException()
        {
            
            var userId = string.Empty;
            var paymentInfoDto = new PaymentInfoDto();

           
            await Assert.ThrowsAsync<ArgumentException>(() => 
                _updatePaymentInfo.ExecuteAsync(userId, paymentInfoDto));
            
            _mapperMock.Verify(x => x.Map<PaymentInfo>(It.IsAny<PaymentInfoDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(It.IsAny<PaymentInfo>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(
                It.IsAny<string>(), It.IsAny<PaymentInfo>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullPaymentInfo_ShouldThrowArgumentNullException()
        {
            
            var userId = "test-user-id";
            PaymentInfoDto? paymentInfoDto = null;

            
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _updatePaymentInfo.ExecuteAsync(userId, paymentInfoDto!));
            
            _mapperMock.Verify(x => x.Map<PaymentInfo>(It.IsAny<PaymentInfoDto>()), Times.Never);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(It.IsAny<PaymentInfo>()), Times.Never);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(
                It.IsAny<string>(), It.IsAny<PaymentInfo>()), Times.Never);
        }

        [Fact]
        public async Task ExecuteAsync_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            
            var userId = "test-user-id";
            var paymentInfoDto = new PaymentInfoDto
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                ExpiryDate = "12/25",
                CVV = "123"
            };
            var paymentEntity = new PaymentInfo
            {
                CardNumber = "1234567890123456",
                CardHolderName = "John Doe",
                ExpiryDate = "12/25",
                CVV = "123"
            };
            var expectedError = new Exception("Database error");

            _mapperMock.Setup(x => x.Map<PaymentInfo>(paymentInfoDto))
                .Returns(paymentEntity);

            _validationServiceMock.Setup(x => x.ValidatePaymentInfoAsync(paymentEntity))
                .ReturnsAsync(new ValidationResult());

            _userRepositoryMock.Setup(x => x.UpdatePaymentInfoAsync(userId, paymentEntity))
                .ThrowsAsync(expectedError);

            
            var exception = await Assert.ThrowsAsync<Exception>(() => 
                _updatePaymentInfo.ExecuteAsync(userId, paymentInfoDto));
            
            Assert.Same(expectedError, exception);
            _mapperMock.Verify(x => x.Map<PaymentInfo>(paymentInfoDto), Times.Once);
            _validationServiceMock.Verify(x => x.ValidatePaymentInfoAsync(paymentEntity), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdatePaymentInfoAsync(userId, paymentEntity), Times.Once);
        }
    }
}