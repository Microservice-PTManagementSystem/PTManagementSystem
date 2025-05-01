using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;
using System.Linq;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdateUserProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserValidationService _validationService;

        public UpdateUserProfile(
            IUserRepository userRepository,
            IMapper mapper,
            IUserValidationService validationService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task ExecuteAsync(string userId, UserProfileDto userProfileDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (userProfileDto == null)
                throw new ArgumentNullException(nameof(userProfileDto), "User profile cannot be null.");

            var userProfileEntity = _mapper.Map<UserProfile>(userProfileDto);

            // Validate the user profile
            var validationResult = await _validationService.ValidateUserProfileAsync(userProfileEntity);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid user profile: {errorMessages}");
            }

            await _userRepository.UpdateUserProfileAsync(userId, userProfileEntity);
        }
    }
}