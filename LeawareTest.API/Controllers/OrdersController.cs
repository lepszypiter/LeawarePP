using LeawareTest.Application;
using LeawareTest.Application.OrderQuery;
using LeawareTest.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeawareTest.API.Controllers;

public record OrderDto(OrderId Id, string ProductName, int Quantity, decimal Price);

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ISender _sender;

    public OrdersController(ILogger<OrdersController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet]
    public async Task<PagedResponse<OrderDto>>? ReadAllOrders(
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("GET: {Name} {ID}", nameof(ReadAllOrders), $"{page} {pageSize}");
        var orders = await _sender.Send(
            new ReadOrdersQuery(page, pageSize),
            cancellationToken);

        var categoryDtos = orders.Data.Select(c => new OrderDto(c.Id, c.Details.ProductName, c.Details.Quantity, c.Details.Price))
            .ToList();

        return new PagedResponse<OrderDto>(categoryDtos, orders.TotalCount, orders.Page, orders.PageSize);
    }
}
