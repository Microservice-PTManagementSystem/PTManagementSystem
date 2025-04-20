namespace PTManagementSystem.Application.Interfaces
{
    public interface IKeycloakService
    {
        Task<string> GetAccessTokenAsync(string username, string password);
        Task<bool> CreateUserAsync(string username, string email, string password);
        Task<bool> UpdateUserAsync(string userId, Dictionary<string, object> attributes);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<bool> RemoveRoleAsync(string userId, string role);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task<bool> ValidateTokenAsync(string token);
        Task<string> GetUserIdByEmailAsync(string email);
        // Task<bool> LogoutAsync(string refreshToken);
    }
}