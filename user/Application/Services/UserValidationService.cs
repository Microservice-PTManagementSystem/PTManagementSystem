using FluentValidation.Results;
using PTManagementSystem.Application.Validators;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Services
{
    public class UserValidationService : IUserValidationService
    {
        private readonly UserValidator _userValidator;
        private readonly UserProfileValidator _userProfileValidator;
        private readonly PaymentInfoValidator _paymentInfoValidator;
        private readonly TrainerProfileValidator _trainerProfileValidator;

        public UserValidationService()
        {
            _userValidator = new UserValidator();
            _userProfileValidator = new UserProfileValidator();
            _paymentInfoValidator = new PaymentInfoValidator();
            _trainerProfileValidator = new TrainerProfileValidator();
        }

        public virtual Task<ValidationResult> ValidateUserExistsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserId", "User ID cannot be null or empty.")
                }));
            }
            return Task.FromResult(new ValidationResult());
        }

        public virtual Task<ValidationResult> ValidateUserTypeAsync(string userId, string expectedType)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserId", "User ID cannot be null or empty.")
                }));
            }
            return Task.FromResult(new ValidationResult());
        }

        public virtual async Task<ValidationResult> ValidateUserProfileAsync(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserProfile", "UserProfile cannot be null.")
                });
            }
            return await _userProfileValidator.ValidateAsync(userProfile);
        }

        public virtual async Task<ValidationResult> ValidatePaymentInfoAsync(PaymentInfo paymentInfo)
        {
            if (paymentInfo == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("PaymentInfo", "PaymentInfo cannot be null.")
                });
            }
            return await _paymentInfoValidator.ValidateAsync(paymentInfo);
        }

        public virtual async Task<ValidationResult> ValidateTrainerProfileAsync(TrainerProfile trainerProfile)
        {
            if (trainerProfile == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("TrainerProfile", "TrainerProfile cannot be null.")
                });
            }
            return await _trainerProfileValidator.ValidateAsync(trainerProfile);
        }

        public virtual Task<ValidationResult> ValidateDeleteUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserId", "User ID cannot be null or empty.")
                }));
            }
            return Task.FromResult(new ValidationResult());
        }
    }
}
