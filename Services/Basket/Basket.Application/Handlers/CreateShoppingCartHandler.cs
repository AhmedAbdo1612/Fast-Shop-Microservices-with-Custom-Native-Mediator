using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers;

public class CreateShoppingCartHandler(IBasketRepository basketRepository, DiscountGrpcService discount)
    : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
{
    public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request,
        CancellationToken cancellationToken)
    {
       Console.WriteLine($"{request} ===> ");
        var cart = request.ToEntity();
        var couponTasks = cart.Items.Select(async item =>
        {
            var coupon = await discount.GetCoupon(item.ProductName);
            item.Price -= coupon.Amount;
        });
        await Task.WhenAll(couponTasks);
        Console.WriteLine($"Creating shopping cart: {cart}");
        var savedCart = await basketRepository.UpsertBasket(cart, cancellationToken);
        Console.WriteLine(savedCart);
        return savedCart.ToResponse();
    }
}