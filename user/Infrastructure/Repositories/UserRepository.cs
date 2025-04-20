using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> DeleteTrainerProfileAsync(string trainerId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentInfoDto> GetPaymentInfoAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<TrainerProfileDto> GetTrainerProfileAsync(string trainerId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentInfoDto> UpdatePaymentInfoAsync(string userId, PaymentInfoDto paymentDto)
        {
            throw new NotImplementedException();
        }

        public Task<TrainerProfileDto> UpdateTrainerProfileAsync(string trainerId, TrainerProfileDto trainerDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> UpdateUserAsync(string userId, UpdateUserDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileDto> UpdateUserProfileAsync(string userId, UserProfileDto profileDto)
        {
            throw new NotImplementedException();
        }
    }
}
