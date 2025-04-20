using PTManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Presentation.DTOs
{
    public class RegisterUserDto
    {
        

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public UserProfileDto Profile { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
        public TrainerProfileDto TrainerProfile { get; set; }
    }
}