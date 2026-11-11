using System.Data;
using Discount.Application.Repository;
using Discount.Infrstructure.Repositories;
using Discount.Infrstructure.Settings;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.Api.DependencyInjection;

public static class InjectDatabase
{
    public static IServiceCollection InjectDatabaseServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>() ??
                               throw new ArgumentNullException($"{nameof(DatabaseSettings)} is required");
        services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));
        services.AddScoped<IDbConnection>(sp =>

            {
                var settings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;

                if (string.IsNullOrEmpty(settings.ConnectionString))
                {
                    throw new ArgumentNullException(nameof(settings.ConnectionString),
                        "Database connection string is required.");
                }

                return new NpgsqlConnection(databaseSettings.ConnectionString);
            }
        );
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        return services;
    }
}