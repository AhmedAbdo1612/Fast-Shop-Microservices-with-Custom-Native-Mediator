using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers;

public static class BrandMapper
{
    public static BrandResponse ToResponse(this ProductBrand brand) =>
        new BrandResponse { Id = brand.Id, Name = brand.Name };

    public static IList<BrandResponse> ToResponseList(this IEnumerable<ProductBrand> brands) =>
        brands.Select(x => x.ToResponse()).ToList();
}