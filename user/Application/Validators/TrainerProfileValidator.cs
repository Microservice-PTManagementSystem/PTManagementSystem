using FluentValidation;
using PTManagementSystem.Domain.Entities;


namespace PTManagementSystem.Application.Validators{
public class TrainerProfileValidator : AbstractValidator<TrainerProfile>
{
    public TrainerProfileValidator()
    {
        RuleForEach(x => x.Specializations).NotEmpty();
        RuleForEach(x => x.Certifications).SetValidator(new CertificationValidator());
        //RuleForEach(x => x.AvailableSlots).SetValidator(new TimeSlotValidator());

        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0);
        RuleFor(x => x.HourlyRate).GreaterThan(0);
    }
}

public class CertificationValidator : AbstractValidator<Certification>
{
    public CertificationValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.IssuingAuthority).NotEmpty();
        RuleFor(x => x.IssueDate).LessThanOrEqualTo(DateTime.Today);
        RuleFor(x => x.ExpiryDate)
            .GreaterThan(x => x.IssueDate)
            .When(x => x.ExpiryDate.HasValue);
    }
}

//public class TimeSlotValidator : AbstractValidator<TimeSlot>
//{
//    public TimeSlotValidator()
//    {
//        RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");
//    }
//}
}