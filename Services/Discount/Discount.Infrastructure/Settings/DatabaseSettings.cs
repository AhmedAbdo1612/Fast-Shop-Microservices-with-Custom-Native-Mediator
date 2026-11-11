namespace Discount.Infrstructure.Settings;

public record DatabaseSettings
{
    public string ConnectionString { get; init; }  = string.Empty;
    
    
}