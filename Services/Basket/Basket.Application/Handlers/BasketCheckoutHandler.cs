using Basket.Application.Commands;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Handlers;

public class BasketCheckoutHandler(
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    ILogger<BasketCheckoutHandler> logger) : IRequestHandler<BasketCheckoutCommand, Unit>
{
    public async Task<Unit> Handle(BasketCheckoutCommand request, CancellationToken ct)
    {
        var basketDto = request.BasketChecoutDto;
        var basketResponse = await mediator.Send(new GetBasketByUserNameQuery(basketDto.UserName),ct);
        if(basketResponse is null || !basketResponse.Items.Any()) throw new InvalidOperationException("Invalid basket");
        var basket = basketResponse.ToEntity();
        var evt = basketDto.ToBasketCheckoutEvent(basket);
        logger.LogInformation("Publishing BasketCheckoutEvent Event");
        await publishEndpoint.Publish(evt, ct);
        await mediator.Send(new DeleteBasketByUserNameCommand(basketDto.UserName), ct);
        return Unit.Value;
    }
}