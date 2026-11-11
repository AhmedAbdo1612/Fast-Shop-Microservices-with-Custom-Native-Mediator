using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetProductsByBrandHandler(IProductRepository productRepository) :IRequestHandler<GetProductsByBrandQuery, IList<ProductResponse>>
{
    public async Task<IList<ProductResponse>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var productList = await productRepository.GetProductsByBrand(request.BrandName);
        return productList.ToResponseList();
    }
}