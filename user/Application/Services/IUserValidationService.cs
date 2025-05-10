using PTManagementSystem.Domain.Entities;
using FluentValidation.Results;

namespace PTManagementSystem.Application.Services
{
    public interface IUserValidationService
    {
        Task<ValidationResult> ValidateUserExistsAsync(string userId);
        Task<ValidationResult> ValidateUserTypeAsync(string userId, string expectedType);
        Task<ValidationResult> ValidateUserProfileAsync(UserProfile userProfile);
        Task<ValidationResult> ValidatePaymentInfoAsync(PaymentInfo paymentInfo);
        Task<ValidationResult> ValidateTrainerProfileAsync(TrainerProfile trainerProfile);
        Task<ValidationResult> ValidateDeleteUserAsync(string userId);
    }
} 