using System.Text.Json;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(IDistributedCache redisCache):IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken ct = default)
    {
       var basket  = await redisCache.GetAsync(userName, ct);
       if(basket == null) return null;
       return JsonSerializer.Deserialize<ShoppingCart>(basket)!;
    }

    public async Task<ShoppingCart> UpsertBasket(ShoppingCart shoppingCart, CancellationToken ct = default)
    {
        await redisCache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize(shoppingCart), ct);
        return await GetBasket(shoppingCart.UserName, ct);
    }

    public async Task DeleteBasket(string userName, CancellationToken ct = default)
    {
        await redisCache.RemoveAsync(userName,ct);
    }
}