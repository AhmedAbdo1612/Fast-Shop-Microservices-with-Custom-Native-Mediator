using Catalog.Core.Entities;

namespace Catalog.Core.Repositories;

public interface IBrandRepository
{
    Task<IEnumerable<ProductBrand>> GetAllAsync(CancellationToken cancellationToken);
    Task<ProductBrand?> GetById(string brandId, CancellationToken cancellationToken);
}