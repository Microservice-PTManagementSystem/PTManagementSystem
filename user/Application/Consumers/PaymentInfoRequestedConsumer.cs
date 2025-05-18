using System.Threading.Tasks;
using PTManagementSystem.Domain.Events;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using MongoDB.Driver;

namespace PTManagementSystem.Application.Consumers
{
    public class PaymentInfoRequestedConsumer
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IMongoCollection<User> _userCollection;

        public PaymentInfoRequestedConsumer(IMessageBroker messageBroker, IMongoCollection<User> userCollection)
        {
            _messageBroker = messageBroker;
            _userCollection = userCollection;
        }

        public async Task Handle(PaymentInfoRequested @event)
        {
            var user = await _userCollection.Find(u => u.KeycloakId == @event.KeycloakId).FirstOrDefaultAsync();
            
            if (user != null)
            {
                var paymentInfoSentEvent = new PaymentInfoSentEvent(user.KeycloakId, user.PaymentInfo);
                await _messageBroker.PublishAsync(paymentInfoSentEvent);
            }
        }
    }
}