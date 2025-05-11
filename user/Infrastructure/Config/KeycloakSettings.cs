namespace PTManagementSystem.Infrastructure.Config
{
    public class KeycloakSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Realm { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string AdminUsername { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
    }
}
