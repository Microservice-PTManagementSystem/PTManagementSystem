using FluentValidation;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Validators
{
    public class UserProfileValidator : AbstractValidator<UserProfile>
    {
        public UserProfileValidator()
        {
            //RuleFor(x => x.FirstName)
            //    .NotEmpty().WithMessage("First name is required")
            //    .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            //RuleFor(x => x.LastName)
            //    .NotEmpty().WithMessage("Last name is required")
            //    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .MaximumLength(20).WithMessage("Gender cannot exceed 20 characters");

            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Height must be greater than 0");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Weight must be greater than 0");

            RuleFor(x => x.FitnessGoals)
                .MaximumLength(500).WithMessage("Fitness goals cannot exceed 500 characters");

            RuleFor(x => x.MedicalConditions)
                .MaximumLength(500).WithMessage("Medical conditions cannot exceed 500 characters");

            RuleFor(x => x.Allergies)
                .MaximumLength(500).WithMessage("Allergies cannot exceed 500 characters");

            // Address validation rules
            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(100).WithMessage("Street cannot exceed 100 characters");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(50).WithMessage("City cannot exceed 50 characters");

            RuleFor(x => x.Address.State)
                .NotEmpty().WithMessage("State is required")
                .MaximumLength(50).WithMessage("State cannot exceed 50 characters");

            RuleFor(x => x.Address.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters");

            RuleFor(x => x.Address.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters");
        }
    }
}