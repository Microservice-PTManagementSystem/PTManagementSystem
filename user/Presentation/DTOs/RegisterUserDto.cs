using PTManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Presentation.DTOs
{
    public class RegisterUserDto
    {
        public string KeycloakUserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public UserType UserType { get; set; }
    }
}