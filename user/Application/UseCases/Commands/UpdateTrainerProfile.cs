using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdateTrainerProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserValidationService _validationService;

        public UpdateTrainerProfile(
            IUserRepository userRepository,
            IMapper mapper,
            IUserValidationService validationService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task ExecuteAsync(string userId, TrainerProfileDto trainerProfileDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (trainerProfileDto == null)
                throw new ArgumentNullException(nameof(trainerProfileDto), "Trainer profile cannot be null.");

            var trainerProfileEntity = _mapper.Map<TrainerProfile>(trainerProfileDto);

            // Validate the trainer profile
            var validationResult = await _validationService.ValidateTrainerProfileAsync(trainerProfileEntity);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid trainer profile: {errorMessages}");
            }

            await _userRepository.UpdateTrainerProfileAsync(userId, trainerProfileEntity);
        }
    }
}
