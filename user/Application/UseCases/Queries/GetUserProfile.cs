using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace user.Application.UseCases.Queries
{
    public class GetUserProfile
    {
        private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        public GetUserProfile(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<UserProfileDto> ExecuteAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));


            return await _userRepository.GetUserProfileAsync(userId);
        }
    }
}
