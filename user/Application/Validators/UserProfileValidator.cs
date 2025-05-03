using FluentValidation;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Validators{
public class UserProfileValidator : AbstractValidator<UserProfile>
{
    public UserProfileValidator()
    {
        RuleFor(x => x.PersonalInfo).SetValidator(new PersonalInfoValidator());
        RuleFor(x => x.ContactInfo).SetValidator(new ContactInfoValidator());
        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }
}

public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
{
    public PersonalInfoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.Gender).NotEmpty();
    }
}

public class ContactInfoValidator : AbstractValidator<ContactInfo>
{
    public ContactInfoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty().Matches(@"^\+?[0-9\s\-]{7,15}$").WithMessage("Invalid phone number.");
    }
}
}