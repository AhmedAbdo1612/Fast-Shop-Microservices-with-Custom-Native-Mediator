using FluentValidation;
using Ordering.Application.Abstractions;

namespace Ordering.Application.Orders.CreateOrder;

public record CreateOrderCommand(
    string UserName,
    decimal TotalPrice,
    string FirstName,
    string LastName,
    string EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode,
    string CardName,
    string CardNumber,
    string Cvv,
    string Expiration,
    int PaymentMethod):ICommand<int>;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
       RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(70).WithMessage("Username must not exceed 70 characters.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0).WithMessage("Total Price must be greater than zero.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name is required.")
            .MaximumLength(50).WithMessage("First Name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name is required.")
            .MaximumLength(50).WithMessage("Last Name must not exceed 50 characters.");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Email Address is not a valid email format.");

        RuleFor(x => x.AddressLine)
            .NotEmpty().WithMessage("Address Line is required.")
            .MaximumLength(150).WithMessage("Address Line must not exceed 150 characters.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required.")
            .MaximumLength(50).WithMessage("Country must not exceed 50 characters.");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required.")
            .MaximumLength(50).WithMessage("State must not exceed 50 characters.");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip Code is required.")
            .MaximumLength(20).WithMessage("Zip Code must not exceed 20 characters.");

        // 3. بيانات الدفع (Payment Details)
        RuleFor(x => x.CardName)
            .NotEmpty().WithMessage("Card Name is required.")
            .MaximumLength(100).WithMessage("Card Name must not exceed 100 characters.");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card Number is required.")
            .CreditCard().WithMessage("Card Number is not a valid credit card format."); // يمكنك استخدام CreditCard() المدمجة أو RegularExpression

        RuleFor(x => x.Cvv)
            .NotEmpty().WithMessage("CVV is required.")
            .Length(3, 4).WithMessage("CVV must be 3 or 4 digits.");

        RuleFor(x => x.Expiration)
            .NotEmpty().WithMessage("Expiration date is required.")
            .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$")
            .WithMessage("Expiration date must be in a valid MM/YY format.");

        RuleFor(x => x.PaymentMethod)
            .GreaterThan(0).WithMessage("Payment Method is required.");
    }
}