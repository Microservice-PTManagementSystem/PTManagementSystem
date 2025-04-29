using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Presentation.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string KeycloakId { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public UserType UserType { get; set; }
        public UserProfileDto Profile { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
        public TrainerProfileDto TrainerProfile { get; set; }
    }
}