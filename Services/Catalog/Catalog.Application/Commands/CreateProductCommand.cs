using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using FluentValidation;
using MediatR;
using MongoDB.Bson;

namespace Catalog.Application.Commands;

public record CreateProductCommand(
    CreateProductDto Model) : IRequest<ProductResponse>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(m => m.Model).NotNull();
        RuleFor(m => m.Model.Name)
            .MinimumLength(5)
            .WithMessage("The product name must be at least 3 characters long.")
            .MaximumLength(50)
            .WithMessage("The product name cannot exceed 50 characters.");
        RuleFor(m => m.Model.BrandId)
            .NotEmpty()
            .WithMessage("The brand id cannot be empty.")
            .Must(IsValidOnjectId)
            .WithMessage("The brand id must be valid id.");
        
        RuleFor(m => m.Model.TypeId)
            .NotEmpty()
            .WithMessage("The type id cannot be empty.")
            .Must(IsValidOnjectId)
            .WithMessage("The type id must be valid id.");
        
    }
    private bool IsValidOnjectId(string id)
    {
        return ObjectId.TryParse(id, out _);
    }
}