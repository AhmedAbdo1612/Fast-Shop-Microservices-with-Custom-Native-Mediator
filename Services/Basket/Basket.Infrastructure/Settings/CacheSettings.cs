namespace Basket.Infrastructure.Settings;

public record CacheSettings
{
    public string ConnectionString { get; init; } = string.Empty;
}