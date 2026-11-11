using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data.SeedData;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Settings;
using MongoDB.Driver;

namespace Catalog.API.DependencyInjection;

public static class InjectMongoDatabase
{
    public static IServiceCollection AddMongoDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
        services.AddSingleton<IMongoClient>(_=>new MongoClient(mongoDbSettings.ConnectionString));
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var cleanDb = sp.GetRequiredService<IMongoClient>();
            return cleanDb.GetDatabase(mongoDbSettings.DatabaseName);
        });
        services.AddScoped<ITypeRepository, TypeRespository>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return new TypeRespository(db, mongoDbSettings);
        });
        services.AddScoped<IBrandRepository,BrandRepository>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return new BrandRepository(db, mongoDbSettings);
        });

        services.AddScoped<IProductRepository, ProductRepository>(sp =>
        {
            var db = sp.GetRequiredService<IMongoDatabase>();
            return new ProductRepository(db, mongoDbSettings);
        } );
       
        return services;
    }
}