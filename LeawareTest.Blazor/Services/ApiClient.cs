namespace LeawareTest.Blazor.Services;


public record ApiOrderDto(string Id, string ProductName, int Quantity, double Price);
public record PagedResponse<T>(IReadOnlyCollection<T> Data, int TotalCount, int Page, int PageSize);
public record OrderDto(Guid OrderId, string Product, int Count, decimal Price);

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task<T?> GetAsync<T>(string endpoint)
    {
        return await _httpClient.GetFromJsonAsync<T>(endpoint);
    }

    public async Task<(IReadOnlyCollection<OrderDto> orders, int totalPages)> LoadOrders(int page, int pageSize)
    {
        var response = await GetAsync<PagedResponse<ApiOrderDto>>($"api/orders?page={page}&pageSize={pageSize}");

        if (response?.Data != null)
        {
            var orders = response.Data.Select(order => new OrderDto(
                Guid.Parse(order.Id),
                order.ProductName,
                order.Quantity,
                (decimal)order.Price
            )).ToList();

            var totalPages = (int)Math.Ceiling((double)response.TotalCount / pageSize);

            return (orders, totalPages);
        }

        return ([], 0);
    }
}
