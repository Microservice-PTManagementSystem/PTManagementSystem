using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Enums;
using PTManagementSystem.Domain.EventModels;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Application.Services;
using FluentValidation.Results;
using System.Linq;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class DeleteUser
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IMessageBroker _messageBroker;
        private readonly IUserValidationService _validationService;

        public DeleteUser(
            IKeycloakService keycloakService,
            IMessageBroker messageBroker,
            IUserValidationService validationService)
        {
            _keycloakService = keycloakService;
            _messageBroker = messageBroker;
            _validationService = validationService;
        }

        public async Task<bool> ExecuteAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            var userId = await _keycloakService.GetUserIdByEmailAsync(email);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("A user with the specified email was not found.");

            // Validate the user ID
            var validationResult = await _validationService.ValidateDeleteUserAsync(userId);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"Invalid user ID: {errorMessages}");
            }

            // Get user roles
            var roles = await _keycloakService.GetUserRolesAsync(userId);
            bool isTrainer = roles.Any(r => r.Equals(UserType.TRAINER.ToString(), StringComparison.OrdinalIgnoreCase));

            var deleted = await _keycloakService.DeleteUserAsync(userId);
            if (!deleted)
                throw new ApplicationException("User could not be deleted. Negative response from Keycloak.");

            // If user is a trainer, publish event
            if (isTrainer)
            {
                var trainerId = Guid.Parse(userId);
                var trainerDeletedEvent = new TrainerDeletedEvent(trainerId);
                await _messageBroker.PublishAsync(trainerDeletedEvent);
            }

            return true;
        }
    }
}


