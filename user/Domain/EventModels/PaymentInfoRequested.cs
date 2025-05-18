using System;

namespace PTManagementSystem.Domain.Events
{
    public class PaymentInfoRequested
    {
        public string KeycloakId { get; }

        public PaymentInfoRequested(string keycloakId)
        {
            KeycloakId = keycloakId;
        }
    }
} 