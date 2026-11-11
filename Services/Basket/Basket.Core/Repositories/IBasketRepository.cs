using Basket.Core.Entities;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName,CancellationToken ct = default);
    Task<ShoppingCart> UpsertBasket(ShoppingCart shoppingCart, CancellationToken ct = default);
    Task DeleteBasket(string userName, CancellationToken ct = default);
}