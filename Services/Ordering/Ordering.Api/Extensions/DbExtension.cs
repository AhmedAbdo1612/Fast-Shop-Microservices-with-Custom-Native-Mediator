using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace Ordering.Api.Extensions;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();
            try
            {
                logger.LogInformation("Migrating database");
                var retry = Policy.Handle<SqlException>().WaitAndRetry(retryCount: 5,
                    sleepDurationProvider: atempt => TimeSpan.FromSeconds(Math.Pow(2, atempt)),
                    onRetry: (exception, timeSpan, count) =>
                    {
                        logger.LogError(exception, "Database migration error occurred");
                    });
                retry.Execute(() => CallSeeder(seeder, context, services));
                logger.LogInformation("Migrated database");
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while migrating database");
            }
        }


        return host;
    }

    private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}