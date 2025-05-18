using FluentValidation;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Validators
{
    public class PaymentInfoValidator : AbstractValidator<PaymentInfo>
    {
        public PaymentInfoValidator()
        {
            RuleFor(x => x.CardHolderName)
                .NotEmpty().WithMessage("Card holder name is required")
                .MaximumLength(100).WithMessage("Card holder name cannot exceed 100 characters");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .CreditCard().WithMessage("Invalid card number");

            /*RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required")
                .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$").WithMessage("Expiry date must be in MM/YY format");
*/
            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^[0-9]{3,4}$").WithMessage("CVV must be 3 or 4 digits");

            RuleFor(x => x.BillingAddress)
                .NotEmpty().WithMessage("Billing address is required")
                .MaximumLength(200).WithMessage("Billing address cannot exceed 200 characters");
        }
    }
}