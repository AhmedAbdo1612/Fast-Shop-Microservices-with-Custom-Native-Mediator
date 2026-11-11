using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Mediator;
using Ordering.Application.Orders.GetOrders;
using Mapster;
using Ordering.Application.Orders.CreateOrder;
using Ordering.Application.Orders.DeleteOrder;
using Ordering.Application.Orders.UpdateOrder;

namespace Ordering.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpGet("{username}", Name = "GetOrdersByUserName")]
    public async Task<IActionResult> GetOrdersByUserName(string username, CancellationToken cancellationToken = default)
    {
        var query = new GetOrderListQuery(username);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost(Name = "CheckoutOrder")]
    public async Task<IActionResult> ChecktoutOrder([FromBody] CreateOrderDto dto,
        CancellationToken cancellationToken = default)
    {
        var command = dto.Adapt<CreateOrderCommand>();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut( Name = "UpdateOrder")]
    public async Task<IActionResult> UpdateOrder([FromBody] OrderDto orderDto,
        CancellationToken cancellationToken = default)
    {
        var command = orderDto.Adapt<UpdateOrderCommand>();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}", Name = "DeleteOrder")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken ct = default)
    {
        var command = new DeleteOrderCommand(id);
         await mediator.Send(command, ct);
        return Ok();
    }
}