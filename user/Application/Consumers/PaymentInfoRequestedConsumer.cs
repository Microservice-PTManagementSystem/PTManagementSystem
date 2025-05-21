using System.Threading.Tasks;
using PTManagementSystem.Domain.Events;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using MongoDB.Driver;
using AutoMapper;

namespace PTManagementSystem.Application.Consumers
{
    public class PaymentInfoRequestedConsumer
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PaymentInfoRequestedConsumer(
            IMessageBroker messageBroker,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _messageBroker = messageBroker;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task Handle(PaymentInfoRequested @event)
        {
            var paymentInfo = await _userRepository.GetPaymentInfoAsync(@event.userId);
            
            if (paymentInfo != null)
            {
                
                var paymentInfoDto = _mapper.Map<PaymentInfoDto>(paymentInfo);

                var paymentInfoSentEvent = new PaymentInfoSentEvent(@event.userId, paymentInfoDto);
                await _messageBroker.PublishAsync(paymentInfoSentEvent);
            }
        }
    }
}
