using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers;

public static class TypeMapper
{
    public static TypeResponse ToResponse(this ProductType productType) =>
        new TypeResponse { Id = productType.Id, Name = productType.Name };

    public static IList<TypeResponse> ToResponseList(this IEnumerable<ProductType> productTypes) => productTypes
        .Select(productType => new TypeResponse { Id = productType.Id, Name = productType.Name }).ToList();
}