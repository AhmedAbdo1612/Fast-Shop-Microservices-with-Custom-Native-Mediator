using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using Mapster;

namespace Basket.Application.Mappers;

public static class BasketMapper
{
    public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
    {
        return new ShoppingCartResponse
        {
            UserName = shoppingCart.UserName,
            Items = shoppingCart.Items.Select(item =>
                new ShoppingCartItemResponse(
                    item.Quantity,
                    item.ImageFile,
                    item.Price,
                    item.ProductId,
                    item.ProductName)).ToList(),
        };
    }

    public static ShoppingCart ToEntity(this CreateShoppingCartCommand shoppingCart)
    {
        return new ShoppingCart
        {
            UserName = shoppingCart.UserName,
            Items = shoppingCart.Items.Select(it => new ShoppingCartItem
            {
                ImageFile = it.ImageFile,
                Price = it.Price,
                ProductId = it.ProductId,
                ProductName = it.ProductName,
                Quantity = it.Quantity
            }).ToList()
        };
    }

    public static readonly Func<ShoppingCart, ShoppingCartResponse> MapCart = cart =>
        new ShoppingCartResponse
        {
            UserName = cart.UserName,
            Items = cart.Items.Select(item =>
                new ShoppingCartItemResponse(
                    item.Quantity,
                    item.ImageFile,
                    item.Price,
                    item.ProductId,
                    item.ProductName)).ToList(),
        };


    public static ShoppingCart ToEntity(this ShoppingCartResponse response)
    {
        return response.Adapt<ShoppingCart>();
    }

    public static BasketCheckoutEvent ToBasketCheckoutEvent(this BasketChecoutDto dto, ShoppingCart shoppingCart)
    {
        var evt = dto.Adapt<BasketCheckoutEvent>();
        evt.UserName = shoppingCart.UserName;
        return evt;
    }
}