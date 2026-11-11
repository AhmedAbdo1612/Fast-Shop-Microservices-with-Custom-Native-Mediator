using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class TypeRespository : ITypeRepository
{
    private IMongoCollection<ProductType> _productTypesCollection;

    public TypeRespository(IMongoDatabase database, MongoDbSettings settings)
    {
        _productTypesCollection = database.GetCollection<ProductType>(settings.BrandCollectionName);
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _productTypesCollection.Find(new BsonDocument()).ToListAsync(cancellationToken);
    }

    public Task<ProductType> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _productTypesCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }
}