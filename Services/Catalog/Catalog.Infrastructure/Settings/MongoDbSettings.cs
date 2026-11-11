namespace Catalog.Infrastructure.Settings;

public record MongoDbSettings
{
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
    public string ProductCollectionName { get; init; }
    public string BrandCollectionName { get; init; }
    public string TypeCollectionName { get; init; }
}