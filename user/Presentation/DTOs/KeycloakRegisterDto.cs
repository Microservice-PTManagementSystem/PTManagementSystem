namespace PTManagementSystem.Presentation.DTOs
{
    public class KeycloakRegisterDto
    {
       // public string KeycloakUserId { get; set; } = string.Empty;
       public required string token { get; set; }
       public required string role { get; set; }
    }
}
