using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductCommandHandler(IProductRepository productRepository) : IRequestHandler<CreateProductCommand,ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brand = await productRepository.GetBrandById(request.Model.BrandId, cancellationToken);
        var type = await productRepository.GetTypeById(request.Model.TypeId, cancellationToken);
      if  (brand is null || type is null)  throw new ApplicationException("Invalid brand id or type id");
      var productEntity = request.Model.ToEntity(brand, type);
  
      var savedProduct = await productRepository.CreateProduct(productEntity, cancellationToken);
      return savedProduct.ToResponse();
    }
}