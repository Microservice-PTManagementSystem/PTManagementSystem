using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace user.Application.UseCases.Commands
{
    public class LoginUser
    {
        private readonly IKeycloakService _keycloakService;

        public LoginUser(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        public async Task<string> ExecuteAsync(LoginDto dto)
        {
            try
            {
                var accessToken = await _keycloakService.GetAccessTokenAsync(dto.Email, dto.Password);
                return accessToken;
            }
            catch (ApplicationException ex)
            {
                throw new UnauthorizedAccessException("Login failed: " + ex.Message);
            }
        }
    }

}

