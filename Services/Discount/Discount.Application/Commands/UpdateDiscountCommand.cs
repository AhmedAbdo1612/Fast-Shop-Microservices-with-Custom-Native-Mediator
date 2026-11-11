using Discount.Application.DTOs;
using FluentValidation;
using MediatR;

namespace Discount.Application.Commands;

public record UpdateDiscountCommand(int Id, string ProductName, string Description, int Amount) : IRequest<CouponDto>;

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MinimumLength(5)
            .WithMessage("Description is too short")
            .MaximumLength(250)
            .WithMessage("Description is too long");
        RuleFor(x => x.ProductName)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MinimumLength(5)
            .WithMessage("Product name is too short")
            .MaximumLength(100)
            .WithMessage("Product name is too long");
        RuleFor(x => x.Amount)
            .Must(y => y > 0)
            .WithMessage("Amount must be greater than zero");
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id cannot be negative");
    }
}