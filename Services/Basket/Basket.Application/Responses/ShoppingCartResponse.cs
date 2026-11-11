using Basket.Core.Entities;

namespace Basket.Application.Responses;

public record ShoppingCartResponse
{
    public string UserName { get; init; }
    public List<ShoppingCartItemResponse> Items { get; init; } = new();
    public decimal TotalPrice =>Items.Sum(x=>x.Price*x.Quantity);

}
