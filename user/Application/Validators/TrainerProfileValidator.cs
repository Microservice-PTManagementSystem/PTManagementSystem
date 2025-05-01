using FluentValidation;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Validators
{
    public class TrainerProfileValidator : AbstractValidator<TrainerProfile>
    {
        public TrainerProfileValidator()
        {
            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");

            RuleFor(x => x.ExperienceYears)
                .GreaterThanOrEqualTo(0).WithMessage("Experience years must be greater than or equal to 0");

            RuleFor(x => x.Bio)
                .MaximumLength(1000).WithMessage("Bio cannot exceed 1000 characters");

            RuleFor(x => x.HourlyRate)
                .GreaterThan(0).WithMessage("Hourly rate must be greater than 0");

            RuleFor(x => x.AvailableDays)
                .NotNull().WithMessage("Available days cannot be null");

            RuleFor(x => x.AvailableHours)
                .NotNull().WithMessage("Available hours cannot be null");

            RuleForEach(x => x.Certifications).SetValidator(new CertificationValidator());
        }
    }

    public class CertificationValidator : AbstractValidator<Certification>
    {
        public CertificationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Certification name is required")
                .MaximumLength(100).WithMessage("Certification name cannot exceed 100 characters");

            RuleFor(x => x.IssuingAuthority)
                .NotEmpty().WithMessage("Issuing authority is required")
                .MaximumLength(100).WithMessage("Issuing authority cannot exceed 100 characters");

            RuleFor(x => x.IssueDate)
                .NotEmpty().WithMessage("Issue date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Issue date cannot be in the future");
        }
    }
}