using System;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoSentEvent
    {
        public string KeycloakId { get; }
        public PaymentInfo PaymentInfo { get; }

        public PaymentInfoSentEvent(string keycloakId, PaymentInfo paymentInfo)
        {
            KeycloakId = keycloakId;
            PaymentInfo = paymentInfo;
        }
    }
}