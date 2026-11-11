using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await productRepository.GetProduct(request.Model.Id, cancellationToken);
        if (existingProduct == null) throw new KeyNotFoundException($"Product with id {request.Model.Id} not found");
        var brand = await productRepository.GetBrandById(request.Model.BrandId, cancellationToken);
        var type = await productRepository.GetTypeById(request.Model.TypeId, cancellationToken);
        if (brand == null || type == null) throw new ApplicationException("Invalid brand or type");
        var newProduct = existingProduct.ApplyUpdates(request.Model, brand, type);
        await productRepository.UpdateProduct(newProduct, cancellationToken);
        return newProduct.ToResponse();
    }
}