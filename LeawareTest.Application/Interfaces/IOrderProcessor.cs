using LeawareTest.Domain;

namespace LeawareTest.Application.Interfaces;

public record OrderDto(string Product, int Count, decimal Price);
public interface IOrderProcessor
{
    Task<IReadOnlyCollection<OrderDto>> ExtractOrder(string emailBody, CancellationToken cancellationToken);
}
