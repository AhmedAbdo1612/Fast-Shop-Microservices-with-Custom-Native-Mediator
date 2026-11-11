using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.SeedData;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IMongoDatabase database, MongoDbSettings settings,
        CancellationToken cancellationToken = default)
    {
        var brandsCollection = database.GetCollection<ProductBrand>(settings.BrandCollectionName);
        var typesCollection = database.GetCollection<ProductType>(settings.TypeCollectionName);
        var productsCollection = database.GetCollection<Product>(settings.ProductCollectionName);

     
        if ((await brandsCollection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken)) == 0)
        {
            var brands = new List<ProductBrand>
            {
                new ProductBrand { Name = "Apple" },
                new ProductBrand { Name = "Samsung" },
                new ProductBrand { Name = "Sony" },
                new ProductBrand { Name = "Nike" },
                new ProductBrand { Name = "Adidas" }
            };
            await brandsCollection.InsertManyAsync(brands, cancellationToken: cancellationToken);
            Console.WriteLine("Brands seeded successfully.");
        }

   
        if ((await typesCollection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken)) == 0)
        {
            var typesList = new List<ProductType>
            {
                new ProductType { Name = "Electronics" },
                new ProductType { Name = "Clothing" },
                new ProductType { Name = "Footwear" },
                new ProductType { Name = "Accessories" },
                new ProductType { Name = "Home Appliances" }
            };
            await typesCollection.InsertManyAsync(typesList, cancellationToken: cancellationToken);
            Console.WriteLine("Product types seeded successfully.");
        }


        if ((await productsCollection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken)) == 0)
        {
       
            var brands = await brandsCollection.Find(_ => true).ToListAsync(cancellationToken);
            var types = await typesCollection.Find(_ => true).ToListAsync(cancellationToken);

            var products = new List<Product>();
            var random = new Random();

            for (int i = 1; i <= 100; i++)
            {
                // اختيار براند وتايب بشكل عشوائي من القوائم
                var randomBrand = brands[random.Next(brands.Count)];
                var randomType = types[random.Next(types.Count)];

                products.Add(new Product
                {
                    Name = $"Product Item {i}",
                    Summary = $"This is a short summary for product item {i}.",
                    Description = $"Detailed description and specifications for product item number {i}. High quality and reliable.",
                    ImageFile = $"product-{i}.png",
                    Price = Math.Round((decimal)(random.NextDouble() * 900 + 10), 2), // سعر عشوائي بين 10 و 910
                    Brand = randomBrand,
                    Type = randomType,
                    CreatedAt = DateTimeOffset.UtcNow.AddDays(-random.Next(0, 30))
                });
            }

            await productsCollection.InsertManyAsync(products, cancellationToken: cancellationToken);
            Console.WriteLine("100 Products seeded successfully.");
        }
    }
}