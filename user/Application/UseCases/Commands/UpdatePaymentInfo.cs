using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;
using System.Linq;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdatePaymentInfo
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserValidationService _validationService;

        public UpdatePaymentInfo(
            IUserRepository userRepository,
            IMapper mapper,
            IUserValidationService validationService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task ExecuteAsync(string userId, PaymentInfoDto paymentInfoDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (paymentInfoDto == null)
                throw new ArgumentNullException(nameof(paymentInfoDto), "Payment information cannot be null.");

            var paymentEntity = _mapper.Map<PaymentInfo>(paymentInfoDto);

            // Validate the payment info
            var validationResult = await _validationService.ValidatePaymentInfoAsync(paymentEntity);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid payment information: {errorMessages}");
            }

            await _userRepository.UpdatePaymentInfoAsync(userId, paymentEntity);

            // return paymentEntity;
        }

    }
}
