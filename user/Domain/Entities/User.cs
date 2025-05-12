using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Domain.Entities
{
    public class User
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string KeycloakId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public UserType UserType { get; set; }
        public UserProfile UserProfile { get; set; } = new UserProfile();
        public PaymentInfo PaymentInfo { get; set; } = new PaymentInfo();
        public TrainerProfile? TrainerProfile { get; set; }
    }
}