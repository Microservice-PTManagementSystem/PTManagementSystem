using FluentValidation;
using PTManagementSystem.Domain.Entities;


namespace PTManagementSystem.Application.Validators{
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PasswordHash).NotEmpty();

        RuleFor(x => x.Profile).SetValidator(new UserProfileValidator());

        When(x => x.PaymentInfo is not null, () =>
        {
            RuleFor(x => x.PaymentInfo).SetValidator(new PaymentInfoValidator());
        });

        When(x => x.TrainerProfile is not null, () =>
        {
            RuleFor(x => x.TrainerProfile).SetValidator(new TrainerProfileValidator());
        });
    }
}
}