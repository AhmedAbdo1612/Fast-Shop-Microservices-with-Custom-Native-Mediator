using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class BrandRepository:IBrandRepository
{
    private IMongoCollection<ProductBrand> _brands;
    public BrandRepository(IMongoDatabase db, MongoDbSettings settings)
    {
        _brands = db.GetCollection<ProductBrand>(settings.BrandCollectionName);
    }
    public async Task<IEnumerable<ProductBrand>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _brands.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<ProductBrand> GetById(string brandId, CancellationToken cancellationToken)
    {
        return await _brands.Find(x => x.Id == brandId).FirstOrDefaultAsync(cancellationToken);
    }
}