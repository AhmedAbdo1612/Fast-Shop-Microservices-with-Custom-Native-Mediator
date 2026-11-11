using System.Net;
using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class BasketController(IMediator mediator, ILogger<BasketController> logger) : ControllerBase
{
    [HttpGet("{username}")]
    public async Task<IActionResult> GetBasket(string username, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBasketByUserNameQuery(username));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateBasket([FromBody] CreateShoppingCartCommand command,
        CancellationToken ct = default)
    {
        logger.LogInformation($"Creating shopping cart {command}");
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteBasket(string username, CancellationToken ct = default)
    {
        await mediator.Send(new DeleteBasketByUserNameCommand(username), ct);
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Checkout([FromBody] BasketChecoutDto dto, CancellationToken ct = default)
    {
        await mediator.Send(new BasketCheckoutCommand(dto), ct);
        return Ok();
    }
}