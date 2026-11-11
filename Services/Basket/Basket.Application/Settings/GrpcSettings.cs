namespace Basket.Application.Settings;

public record GrpcSettings
{
    public string DiscountUrl { get; init; } = string.Empty;
}
