using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken = default);
    Task<Pagination<Product>?> GetProducts(CatalogSpecParams specParams, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsByName(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsByBrand(string brand, CancellationToken cancellationToken = default);
    Task<Product> GetProduct(string id, CancellationToken cancellationToken = default);
    Task<Product> CreateProduct(Product product, CancellationToken cancellationToken = default);
    Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteProduct(string id, CancellationToken cancellationToken = default);
    Task<ProductBrand> GetBrandById(string brandId, CancellationToken cancellationToken = default);
    Task<ProductType> GetTypeById(string typeId, CancellationToken cancellationToken = default);
    
}