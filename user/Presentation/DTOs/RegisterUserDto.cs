using PTManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PTManagementSystem.Presentation.DTOs
{
    public class RegisterUserDto
    {

        public string KeycloakUserId { get; set; } 
        public string Email { get; set; }          
        public bool IsEmailConfirmed { get; set; }  
        public UserType UserType { get; set; }
    }
}