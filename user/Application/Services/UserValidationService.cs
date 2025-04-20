using FluentValidation.Results;
using PTManagementSystem.Application.Validators;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Services
{
    public class UserValidationService
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

        // CREATE
        public ValidationResult ValidateCreateUser(User user)
        {
            if (user == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("User", "User cannot be null.")
                });
            }

            return _userValidator.Validate(user);
        }

        // DELETE
        public ValidationResult ValidateDeleteUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserId", "User Id cannot be empty.")
                });
            }

            return new ValidationResult();
        }

        // UPDATE
        public ValidationResult ValidateUpdateUser(User user)
        {
            if (user == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("User", "User cannot be null.")
                });
            }

            if (user.Id == Guid.Empty)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("User.Id", "User Id cannot be empty.")
                });
            }

            return _userValidator.Validate(user);
        }

        // LOGIN
        public ValidationResult ValidateLoginUser(string email, string password)
        {
            var failures = new List<ValidationFailure>();

            if (string.IsNullOrWhiteSpace(email))
                failures.Add(new ValidationFailure("Email", "Email is required."));
            else if (!email.Contains("@"))
                failures.Add(new ValidationFailure("Email", "Invalid email format."));

            if (string.IsNullOrWhiteSpace(password))
                failures.Add(new ValidationFailure("Password", "Password is required."));

            return new ValidationResult(failures);
        }

        // RESET PASSWORD
        public ValidationResult ValidateResetPassword(string email, string newPassword)
        {
            var failures = new List<ValidationFailure>();

            if (string.IsNullOrWhiteSpace(email))
                failures.Add(new ValidationFailure("Email", "Email is required."));
            else if (!email.Contains("@"))
                failures.Add(new ValidationFailure("Email", "Invalid email format."));

            if (string.IsNullOrWhiteSpace(newPassword))
                failures.Add(new ValidationFailure("NewPassword", "New password is required."));
            else if (newPassword.Length < 6)
                failures.Add(new ValidationFailure("NewPassword", "Password must be at least 6 characters long."));

            return new ValidationResult(failures);
        }

        
        public ValidationResult ValidateUserProfile(UserProfile profile)
        {
            if (profile == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserProfile", "UserProfile cannot be null.")
                });
            }

            return _userProfileValidator.Validate(profile);
        }

        public ValidationResult ValidatePaymentInfo(PaymentInfo paymentInfo)
        {
            if (paymentInfo == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("PaymentInfo", "PaymentInfo cannot be null.")
                });
            }

            return _paymentInfoValidator.Validate(paymentInfo);
        }

        public ValidationResult ValidateTrainerProfile(TrainerProfile trainerProfile)
        {
            if (trainerProfile == null)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("TrainerProfile", "TrainerProfile cannot be null.")
                });
            }

            return _trainerProfileValidator.Validate(trainerProfile);
        }

        public ValidationResult ValidateUserId(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Id", "User Id cannot be empty.")
                });
            }

            return new ValidationResult();
        }
    }
}
