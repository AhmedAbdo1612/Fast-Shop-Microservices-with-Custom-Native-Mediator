using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{
    public static ProductResponse ToResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Description = product.Description,
            Price = product.Price,
            ImageFile = product.ImageFile,
            CreatedAt = product.CreatedAt,
            Brand = product.Brand,
            Type = product.Type,
        };
    }

    public static IList<ProductResponse> ToResponseList(this IEnumerable<Product> productList) =>
        productList.Select(ToResponse).ToList();

    public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
    {
        return new Pagination<ProductResponse>(pagination.PageIndex, pagination.PageSize, pagination.Count,
            pagination.Data.Select(ToResponse).ToList());
    }

    public static Product ToEntity(this CreateProductDto model, ProductBrand brand ,ProductType type) => new Product
    {
        Name = model.Name,
        Summary = model.Summary,
        Description = model.Description,
        Price = model.Price,
        ImageFile = model.ImageFile,
        CreatedAt = DateTimeOffset.UtcNow,
        Brand = brand,
        Type = type
    };

    public static Product ApplyUpdates(this Product model,UpdateProductDto dto, ProductBrand brand, ProductType type)
    {
        return new Product
        {
            Id = model.Id,
            Name = dto.Name,
            Summary = dto.Summary,
            Description = dto.Description,
            Price = dto.Price,
            ImageFile = dto.ImageFile,
            CreatedAt = model.CreatedAt,
            Brand = brand,
            Type = type
        };
    }
    
}