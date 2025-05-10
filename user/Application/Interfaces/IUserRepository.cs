using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> UpdateUserAsync(string userId, UserDto updateDto);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserProfileDto?> GetUserProfileAsync(string userId);
        Task UpdateUserProfileAsync(string userId, UserProfile profile);
        Task<PaymentInfoDto?> GetPaymentInfoAsync(string userId);
        Task UpdatePaymentInfoAsync(string userId, PaymentInfo paymentInfo);
        Task<TrainerProfileDto?> GetTrainerProfileAsync(string trainerId);
        Task UpdateTrainerProfileAsync(string trainerId, TrainerProfile trainerProfile);
        Task<bool> AddUserAsync(RegisterUserDto registerDto);
        Task<IEnumerable<UserDto>> GetAllTrainersAsync();
    }
}