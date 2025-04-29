using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdatePaymentInfo
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdatePaymentInfo(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(string userId, PaymentInfoDto paymentInfoDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (paymentInfoDto == null)
                throw new ArgumentNullException(nameof(paymentInfoDto), "Payment information cannot be null.");

            var paymentEntity = _mapper.Map<PaymentInfo>(paymentInfoDto);

            await _userRepository.UpdatePaymentInfoAsync(userId, paymentEntity);

            // return paymentEntity;
        }

    }
}
