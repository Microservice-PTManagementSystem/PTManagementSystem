using FluentValidation;
using PTManagementSystem.Domain.Entities;


namespace PTManagementSystem.Application.Validators{
public class PaymentInfoValidator : AbstractValidator<PaymentInfo>
{
    public PaymentInfoValidator()
    {
        RuleFor(x => x.CardHolderName).NotEmpty();
        RuleFor(x => x.CardNumber).CreditCard().WithMessage("Invalid credit card number.");
        RuleFor(x => x.ExpirationMonth).NotEmpty().Matches(@"^(0[1-9]|1[0-2])$").WithMessage("Invalid expiration month.");
        RuleFor(x => x.ExpirationYear).NotEmpty().Matches(@"^\d{4}$").WithMessage("Invalid expiration year.");
        RuleFor(x => x.Cvv).NotEmpty().Matches(@"^\d{3,4}$").WithMessage("Invalid CVV.");
        RuleFor(x => x.BillingAddress).NotEmpty();
    }
}
}