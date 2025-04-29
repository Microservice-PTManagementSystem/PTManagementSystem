using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class UpdateTrainerProfile
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateTrainerProfile(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public async Task ExecuteAsync(string userId, TrainerProfileDto trainerProfileDto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (trainerProfileDto == null)
                throw new ArgumentNullException(nameof(trainerProfileDto), "Trainer profile cannot be null.");

            var trainerProfileEntity = _mapper.Map<TrainerProfile>(trainerProfileDto);

            await _userRepository.UpdateTrainerProfileAsync(userId, trainerProfileEntity);

        }
    }
}
