using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdateUserProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserProfile(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task ExecuteAsync(string userId, UserProfileDto userProfileDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (userProfileDto == null)
                throw new ArgumentNullException(nameof(userProfileDto), "User profile cannot be null.");

            var userProfileEntity = _mapper.Map<UserProfile>(userProfileDto);

            await _userRepository.UpdateUserProfileAsync(userId, userProfileEntity);

        }
    }
}
