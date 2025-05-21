using System;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoSentEvent
    {
        public string KeycloakId { get; }
        public PaymentInfoDto PaymentInfo { get; }

        public PaymentInfoSentEvent(string keycloakId, PaymentInfoDto paymentInfo)
        {
            KeycloakId = keycloakId;
            PaymentInfo = paymentInfo;
        }
    }
}