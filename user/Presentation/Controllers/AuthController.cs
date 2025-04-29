using Microsoft.AspNetCore.Mvc;
using PTManagementSystem.Application.UseCases.Commands;
using PTManagementSystem.Presentation.DTOs;
using user.Application.UseCases.Commands;
using user.Application.UseCases.Queries;
using user.Presentation.DTOs;

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
        private readonly UpdatePaymentInfo _updatePaymentInfo;
        private readonly UpdateTrainerProfile _updateTrainerProfile;
        private readonly UpdateUserProfile _updateUserProfile;
        private readonly GetPaymentInfo _getPaymentInfo;
        private readonly GetTrainerProfile _getTrainerProfile;  
        private readonly GetUserProfile _getUserProfile;
        private readonly GetUser _getUser;

        public AuthController(RegisterUser registerUser, LoginUser loginUser, ILogger<AuthController> logger, DeleteUser deleteUser,
                              UpdatePaymentInfo updatePaymentInfo, UpdateTrainerProfile updateTrainerProfile, UpdateUserProfile updateUserProfile, 
                              GetPaymentInfo getPaymentInfo, GetTrainerProfile getTrainerProfile, GetUserProfile getUserProfile, GetUser getUser)
        {
            _registerUser = registerUser;
            _loginUser = loginUser;
            _deleteUser = deleteUser;
            _logger = logger;
            _updatePaymentInfo = updatePaymentInfo;
            _updateTrainerProfile = updateTrainerProfile;
            _updateUserProfile = updateUserProfile;
            _getPaymentInfo = getPaymentInfo;
            _getTrainerProfile = getTrainerProfile;
            _getUserProfile = getUserProfile;
            _getUser = getUser;
        }

        [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] KeycloakRegisterDto dto)
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
     [HttpPost("Delete/{email}")]
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
        [HttpPut("UpdatePaymentInfo/{userId}")]
        public async Task<IActionResult> UpdatePaymentInfo(string userId, [FromBody] PaymentInfoDto paymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                 await _updatePaymentInfo.ExecuteAsync(userId, paymentDto);
                 return Ok("Successfully added.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating payment info.");
                return StatusCode(500, "Server error.");
            }
        }

        [HttpPut("UpdateTrainerProfile/{userId}")]
        public async Task<IActionResult> UpdateTrainerProfile(string userId, [FromBody] TrainerProfileDto trainerProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _updateTrainerProfile.ExecuteAsync(userId, trainerProfileDto);
                return Ok("Successfully added.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating payment info.");
                return StatusCode(500, "Server error.");
            }
        }

        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _updateUserProfile.ExecuteAsync(userId, userProfileDto);
                return Ok("Successfully added.");
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating payment info.");
                return StatusCode(500, "Server error.");
            }
        }

        [HttpGet("GetUser/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var user = await _getUser.ExecuteAsync(userId);

                if (user == null)
                    return NotFound("User  not found.");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user .");
                return StatusCode(500, "Server error.");
            }
        }



        [HttpGet("GetPaymentInfo/{userId}")]
        public async Task<IActionResult> GetPaymentInfo(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var paymentInfo = await _getPaymentInfo.ExecuteAsync(userId);

                if (paymentInfo == null)
                    return NotFound("Payment info not found.");

                return Ok(paymentInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving payment info.");
                return StatusCode(500, "Server error.");
            }
        }

        [HttpGet("GetTrainerProfile/{userId}")]
        public async Task<IActionResult> GetTrainerProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var trainerProfile = await _getTrainerProfile.ExecuteAsync(userId);

                if (trainerProfile == null)
                    return NotFound("Trainer profile not found.");

                return Ok(trainerProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving trainer profile.");
                return StatusCode(500, "Server error.");
            }
        }

        [HttpGet("GetUserProfile/{userId}")]
        public async Task<IActionResult> GetUserProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var userProfile = await _getUserProfile.ExecuteAsync(userId);

                if (userProfile == null)
                    return NotFound("User profile not found.");

                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user profile.");
                return StatusCode(500, "Server error.");
            }
        }

    }
}