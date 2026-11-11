using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderDbContext dbContext) : RepositoryBase<Order>(dbContext), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Where(x => x.UserName.ToLower() == userName.ToLower())
            .ToListAsync(cancellationToken);
    }
}