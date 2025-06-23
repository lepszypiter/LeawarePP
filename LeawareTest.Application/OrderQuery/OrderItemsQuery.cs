using LeawareTest.BuildingBlocks.Messaging;
using LeawareTest.Domain;

namespace LeawareTest.Application.OrderQuery;

public record ReadOrdersQuery(
    int Page,
    int PageSize
) : IQuery<PagedResponse<ReadOrderDto>>;

public record ReadOrderDto(
    OrderId Id, OrderDetails Details
);

public class OrderItemQueryHandler : IQueryHandler<ReadOrdersQuery,PagedResponse<ReadOrderDto>>
{
    private readonly IOrderRepository _repository;

    public OrderItemQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Task<PagedResponse<ReadOrderDto>> Handle(ReadOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = _repository.ReadOrders(request.Page, request.PageSize, cancellationToken);
        var orderDtos = orders.Result.data
            .Select(o => new ReadOrderDto(o.Id, o.OrderDetails))
            .ToList();
        return Task.FromResult(new PagedResponse<ReadOrderDto>(orderDtos, orders.Result.count, request.Page, request.PageSize));
    }
}
