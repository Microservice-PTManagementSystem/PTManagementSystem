using PTManagementSystem.Domain.Enums;
namespace PTManagementSystem.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string KeycloakId { get; set; }  
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserType UserType { get; set; }
        public UserProfile Profile { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public TrainerProfile? TrainerProfile { get; set; }
    }

}