using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Queries
{
    public class GetTrainerProfile
    {
        private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        public GetTrainerProfile(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<TrainerProfileDto?> ExecuteAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
              throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await _userRepository.GetTrainerProfileAsync(userId); ;
        }
    }
}
