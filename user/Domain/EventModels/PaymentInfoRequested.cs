using System;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoRequested
    {
        public string userId { get; }

        public PaymentInfoRequested(string userId)
        {
            userId = userId;
        }
    }
} 