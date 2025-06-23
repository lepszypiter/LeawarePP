using LeawareTest.Domain;
using LeawareTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LeawareTest.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _dbContext;

    public OrderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddNewOrderitem(OrderItem newOrderItem, CancellationToken cancellationToken)
    {
        await _dbContext.OrderItems.AddAsync(newOrderItem, cancellationToken);
    }

    public async Task<(IReadOnlyCollection<OrderItem> data, int count)> ReadOrders(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.OrderItems.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken: cancellationToken);

        var items = await query
            .OrderBy(x => x.Id) // Assuming OrderDate is a property of OrderItem
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
