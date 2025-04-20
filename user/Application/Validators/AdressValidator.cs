using FluentValidation;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Validators{
public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required.");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.State).NotEmpty().WithMessage("State is required.");
        RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
        RuleFor(x => x.PostalCode).NotEmpty().Matches(@"^\d{5}(-\d{4})?$").WithMessage("Invalid postal code format.");
    }
}
}