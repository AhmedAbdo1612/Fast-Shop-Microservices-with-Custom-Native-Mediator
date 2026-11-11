using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.DTOs;

public record ProductDto(
    string Id,
    string Name,
    string Summary,
    string Description,
    string ImageFile,
    BrandDto Brand,
    TypeDto Type,
    decimal Price,
    DateTimeOffset CreatedAt
);

public record BrandDto(
    string Id,
    string Name
);

public record TypeDto(
    string Id,
    string Name
);

public record class CreateProductDto
{
    [Required] public string Name { get; init; }
    [Required] public string Description { get; init; }
    [Required] public string ImageFile { get; init; }
    [Required] public string Summary { get; init; }
    [Required] public string BrandId { get; init; }
    [Required] public string TypeId { get; init; }

    [Range(0.01, double.MaxValue, ErrorMessage = "You must enter a valid price with minimum value 0.1")]
    public decimal Price { get; init; }
}

public record  UpdateProductDto
{
    [Required] public string Id { get; init; }
    [Required] public string Name { get; init; }
    [Required] public string Description { get; init; }
    [Required] public string ImageFile { get; init; }
    [Required] public string Summary { get; init; }
    [Required] public string BrandId { get; init; }
    [Required] public string TypeId { get; init; }

    [Range(0.01, double.MaxValue, ErrorMessage = "You must enter a valid price with minimum value 0.1")]
    public decimal Price { get; init; }
}