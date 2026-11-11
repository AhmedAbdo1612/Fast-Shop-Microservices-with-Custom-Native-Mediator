using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Ordering.Infrastructure.Data;

public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var builder = new DbContextOptionsBuilder<OrderDbContext>();
       
        var connectionString = configuration["ConnectionStrings:OrderDbConnectionString"]; 
        Console.WriteLine("\n================================>");
        Console.WriteLine(connectionString);
        builder.UseNpgsql(connectionString, opt =>
        {
            opt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
        });
        return new OrderDbContext(builder.Options);
    }
}