using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Queries
{
    public class GetUserProfile
    {
        private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        public GetUserProfile(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<UserProfileDto?> ExecuteAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            var profile = await _userRepository.GetUserProfileAsync(userId);
            if (profile == null)
                throw new KeyNotFoundException($"User profile not found for user ID: {userId}");

            return profile;
        }
    }
}
