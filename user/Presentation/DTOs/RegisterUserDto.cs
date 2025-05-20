using PTManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Presentation.DTOs
{
    public class RegisterUserDto
    { 
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string Name { get; set; } = string.Empty;
        public string KeycloakUserId { get; set; } = string.Empty;
        public UserType Role { get; set; }
    }
}