namespace PTManagementSystem.Presentation.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public UserProfileDto Profile { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
        public TrainerProfileDto TrainerProfile { get; set; }
    }
}