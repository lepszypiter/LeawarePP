namespace LeawareTest.Domain;

public interface IOrderRepository
{
    Task AddNewOrderitem(OrderItem newOrderItem, CancellationToken cancellationToken);
    Task<(IReadOnlyCollection<OrderItem> data, int count)> ReadOrders(int page, int pageSize, CancellationToken cancellationToken);
}
