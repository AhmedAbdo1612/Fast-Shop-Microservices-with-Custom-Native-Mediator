using Discount.Infrstructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static IHost MigrateDatabase(this IHost host)
    {
        Console.WriteLine("The seeding operation started...");
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
        var config = services.GetRequiredService<IConfiguration>();

        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        var builder = new NpgsqlConnectionStringBuilder(databaseSettings.ConnectionString);
        var targetDatabaseName = builder.Database;
        builder.Database = "postgres";
        using var adminConnection = new NpgsqlConnection(builder.ConnectionString);
        adminConnection.Open();
        using var checkCmd = new NpgsqlCommand(
            $"SELECT 1 FROM pg_database WHERE datname = '{targetDatabaseName}';", adminConnection);

        var exists = checkCmd.ExecuteScalar();
        if (exists == null)
        {
            logger.LogInformation("Database '{DatabaseName}' does not exist. Creating it...", targetDatabaseName);
            using var createDbCmd = new NpgsqlCommand($"CREATE DATABASE \"{targetDatabaseName}\";", adminConnection);
            createDbCmd.ExecuteNonQuery();
            logger.LogInformation("Database '{DatabaseName}' created successfully.", targetDatabaseName);
        }

        adminConnection.Close();
        var dbConnection = new NpgsqlConnection(databaseSettings.ConnectionString);

        try
        {
            logger.LogInformation("Migrating database...");

            ApplyMigration(dbConnection, logger);
            logger.LogInformation("Database migrated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }

        return host;
    }

    private static void ApplyMigration(NpgsqlConnection dbConnection, ILogger logger)
    {
        int retry = 5;
        while (retry > 0)
        {
            try
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                {
                    dbConnection.Open();
                }

                using var cmd = new NpgsqlCommand
                {
                    Connection = dbConnection,
                    CommandText = @"
                        CREATE TABLE IF NOT EXISTS Coupons ( 
                            Id SERIAL PRIMARY KEY,
                            ProductName VARCHAR(500) NOT NULL,
                            Description TEXT,
                            Amount INT 
                        );"
                };

                cmd.ExecuteNonQuery();
                using var checkRowsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Coupons;", dbConnection);
                var rowCount = Convert.ToInt32(checkRowsCmd.ExecuteScalar());
                if (rowCount == 0)
                {
                    logger.LogInformation("Coupons table is empty. Seeding initial 10 coupons...");
                    cmd.CommandText = @"
                            INSERT INTO Coupons (ProductName, Description, Amount) VALUES
                            ('iPhone 15 Pro', 'Discount on Apple flagship phone', 100),
                            ('Samsung Galaxy S24', 'Special offer on Samsung device', 80),
                            ('MacBook Pro M3', 'Laptops discount coupon', 150),
                            ('Sony WH-1000XM5', 'Headphones discount', 50),
                            ('Dell UltraSharp Monitor', 'Office accessories discount', 40),
                            ('Logitech MX Master 3S', 'Wireless mouse discount', 20),
                            ('Keychron K2 Keyboard', 'Mechanical keyboard coupon', 25),
                            ('PlayStation 5 Pro', 'Gaming console special discount', 75),
                            ('Xbox Series X', 'Gaming offer coupon', 60),
                            ('Nintendo Switch OLED', 'Handheld console discount', 45);
                        ";
                    cmd.ExecuteNonQuery();
                }
                else logger.LogInformation("Coupons table is already created and has data");
                break;
            }
            catch (Exception ex)
            {
                retry--;
                logger.LogWarning(ex, "Failed to migrate database. Retrying... Attempts left: {Retry}", retry);

                if (retry == 0)
                {
                    throw;
                }

                Thread.Sleep(2000);
            }
            finally
            {
                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
            }
        }
    }
}