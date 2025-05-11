using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Presentation.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserProfileDto Profile { get; set; } = new UserProfileDto();
        public PaymentInfoDto PaymentInfo { get; set; } = new PaymentInfoDto();
        public TrainerProfileDto? TrainerProfile { get; set; }
    }
}