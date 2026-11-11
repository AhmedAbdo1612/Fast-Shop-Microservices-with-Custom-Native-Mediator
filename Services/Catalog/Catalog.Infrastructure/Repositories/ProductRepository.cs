using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Catalog.Infrastructure.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _productsCollection;
    private readonly IMongoCollection<ProductBrand> _brandsCollection;
    private readonly IMongoCollection<ProductType> _typesCollection;

    public ProductRepository(IMongoDatabase database, MongoDbSettings settings)
    {
        _productsCollection = database.GetCollection<Product>(settings.ProductCollectionName);
        _brandsCollection = database.GetCollection<ProductBrand>(settings.ProductCollectionName);
        _typesCollection = database.GetCollection<ProductType>(settings.ProductCollectionName);
    }

    public async Task<IEnumerable<Product>> GetAllProducts(CancellationToken cancellationToken = default)
    {
        return await _productsCollection.Find(x => true).ToListAsync(cancellationToken);
    }

    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams,
        CancellationToken cancellationToken = default)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;
        if (!string.IsNullOrEmpty(specParams.Search))
            filter &= builder.Where(p => p.Name.ToLower().Contains(specParams.Search.ToLower()));

        if (!string.IsNullOrEmpty(specParams.BrandId))
            filter &= builder.Eq(p => p.Brand.Id, specParams.BrandId);

        if (!string.IsNullOrEmpty(specParams.TypeId))
            filter &= builder.Eq(p => p.Type.Id, specParams.TypeId);

        var sort = Builders<Product>.Sort.Ascending(x => x.Price);

        var count = await _productsCollection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var data = await _productsCollection.Find(filter).Sort(sort)
            .Skip(specParams.PageSize * (specParams.PageIndex - 1))
            .Limit(specParams.PageSize)
            .ToListAsync(cancellationToken);

        return new Pagination<Product>(specParams.PageIndex, specParams.PageSize, count, data);
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression($"*{name}.*", "i"));
        return await _productsCollection.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brand,
        CancellationToken cancellationToken = default)
    {
        return await _productsCollection.Find(x => x.Name.ToLower() == brand.ToLower()).ToListAsync(cancellationToken);
    }

    public Task<Product> GetProduct(string id, CancellationToken cancellationToken = default)
    {
        return _productsCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product> CreateProduct(Product product, CancellationToken cancellationToken = default)
    {
        await _productsCollection.InsertOneAsync(product, cancellationToken: cancellationToken);
        return product;
    }

    public async Task<bool> UpdateProduct(Product product, CancellationToken cancellationToken = default)
    {
        var updatedProduct = await _productsCollection.ReplaceOneAsync(x => x.Id == product.Id, product,
            cancellationToken: cancellationToken);
        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id, CancellationToken cancellationToken = default)
    {
        var deletedProduct = await _productsCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public Task<ProductBrand> GetBrandById(string brandId, CancellationToken cancellationToken = default)
    {

        return _brandsCollection.Find(x => x.Id == brandId).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<ProductType> GetTypeById(string typeId, CancellationToken cancellationToken = default)
    {
        return _typesCollection.Find(x => x.Id == typeId).FirstOrDefaultAsync(cancellationToken);
    }
}