using FluentValidation;
using MediatR;

namespace Discount.Application.Commands;

public record DeleteDiscountCommand(string ProductName) : IRequest<bool>;

public class DeleteDiscountCommandValidator : AbstractValidator<DeleteDiscountCommand>
{
    public DeleteDiscountCommandValidator()
    {
        RuleFor(x => x.ProductName)
            .MinimumLength(3)
            .WithMessage("Product name must be at least 3 characters long");
    }
}