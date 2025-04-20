//using PTManagementSystem.Application.DTOs;

//namespace PTManagementSystem.Application.Interfaces
//{
//    public interface IAuthRepository
//    {
//        Task<string> LoginAsync(LoginDTO loginDto);
//        Task<bool> RegisterAsync(RegisterUserDTO registerDto);
//        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
//        Task<bool> ResetPasswordAsync(string email);
//        Task<bool> ValidateTokenAsync(string token);
//        Task<bool> RevokeTokenAsync(string userId);
//        Task<string> RefreshTokenAsync(string refreshToken);
//        Task<bool> ConfirmEmailAsync(string userId, string token);
//    }
//}