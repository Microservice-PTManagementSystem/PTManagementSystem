using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Application.UseCases;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUser _registerUser;
        private readonly LoginUser _loginUser;
         private readonly DeleteUser _deleteUser;
        private readonly ILogger<AuthController> _logger;

           public AuthController(RegisterUser registerUser, LoginUser loginUser, ILogger<AuthController> logger, DeleteUser deleteUser)
    {
        _registerUser = registerUser;
        _loginUser = loginUser;
        _deleteUser=deleteUser;
        _logger = logger;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _registerUser.RegisterAsync(dto);
            return Ok("User successfully registered. Please confirm email.");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during registration.");
            return StatusCode(500, "Server error.");
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var token = await _loginUser.ExecuteAsync(dto);
            return Ok(new { AccessToken = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during login.");
            return StatusCode(500, "Server error.");
        }
    }
     [HttpDelete("Delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var result = await _deleteUser.ExecuteAsync(email);
                return Ok("User successfully deleted.");
                
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while deleting the user.");
                return StatusCode(500, "Server error.");
            }
        }
}
}