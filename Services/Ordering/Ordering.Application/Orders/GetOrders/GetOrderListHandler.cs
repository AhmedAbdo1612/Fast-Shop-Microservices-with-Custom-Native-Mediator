using Ordering.Application.Abstractions;
using Ordering.Application.DTOs;
using Ordering.Application.Mappers;
using Ordering.Core.Repositories;

namespace Ordering.Application.Orders.GetOrders;

public class GetOrderListHandler(IOrderRepository orderRepository) :IRequestHandler<GetOrderListQuery,List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrderListQuery query, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetOrdersByUserName(query.UserName, cancellationToken);
        return orders.Select(x => x.MapToDto()).ToList();
    }
}