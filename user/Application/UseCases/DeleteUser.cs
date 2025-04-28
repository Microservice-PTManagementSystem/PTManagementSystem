using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Enums;
using PTManagementSystem.Domain.EventModels;

namespace PTManagementSystem.Application.UseCases
{
    public class DeleteUser
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IMessageBroker _messageBroker;

        public DeleteUser(IKeycloakService keycloakService, IMessageBroker messageBroker)
        {
            _keycloakService = keycloakService;
            _messageBroker = messageBroker;
        }

        public async Task<bool> ExecuteAsync(string email)
        {
            var userId = await _keycloakService.GetUserIdByEmailAsync(email);
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("A user with the specified email was not found.");

            // Kullanıcının rollerini alma
            var roles = await _keycloakService.GetUserRolesAsync(userId);
            bool isTrainer = roles.Any(r => r.Equals(UserType.TRAINER.ToString(), StringComparison.OrdinalIgnoreCase));

            var deleted = await _keycloakService.DeleteUserAsync(userId);
            if (!deleted)
                throw new ApplicationException("User could not be deleted. Negative response from Keycloak.");

            // Eğer kullanıcı bir trainer ise event publish etme
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


