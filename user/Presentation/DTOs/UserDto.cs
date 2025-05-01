using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Presentation.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string KeycloakId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public UserType UserType { get; set; }
        public UserProfileDto UserProfile { get; set; } = new UserProfileDto();
        public PaymentInfoDto PaymentInfo { get; set; } = new PaymentInfoDto();
        public TrainerProfileDto? TrainerProfile { get; set; }
    }
}