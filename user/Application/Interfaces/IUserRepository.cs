using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto updateDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<UserProfileDto> UpdateUserProfileAsync(string userId, UserProfileDto profileDto);
        Task<PaymentInfoDto> GetPaymentInfoAsync(string userId);
        Task<PaymentInfoDto> UpdatePaymentInfoAsync(string userId, PaymentInfoDto paymentDto);
        Task<TrainerProfileDto> GetTrainerProfileAsync(string trainerId);
        Task<TrainerProfileDto> UpdateTrainerProfileAsync(string trainerId, TrainerProfileDto trainerDto);
        Task<bool> DeleteTrainerProfileAsync(string trainerId);
    }
}