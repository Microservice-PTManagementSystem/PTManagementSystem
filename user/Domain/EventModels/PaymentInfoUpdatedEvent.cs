using System;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoUpdatedEvent 
    {
        public Guid UserId { get; }
        public PaymentInfo PaymentInfo { get; }
        public DateTime OccurredOn { get; }

        public PaymentInfoUpdatedEvent(Guid userId, PaymentInfo paymentInfo)
        {
            UserId = userId;
            PaymentInfo = paymentInfo;
            OccurredOn = DateTime.UtcNow;
        }
    }
}