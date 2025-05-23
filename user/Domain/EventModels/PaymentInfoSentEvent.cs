using System;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoSentEvent
    {
        public string userId { get; }
        public PaymentInfoDto PaymentInfo { get; }

        public PaymentInfoSentEvent(string userId, PaymentInfoDto paymentInfo)
        {
            userId = userId;
            PaymentInfo = paymentInfo;
        }
    }
}