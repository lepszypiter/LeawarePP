namespace LeawareTest.Domain;

public class OrderItem
{
    public OrderId Id { get; set; } = default!;
    public OrderDetails OrderDetails { get; set; } = default!;

    public static OrderItem CreateNewOrderItem(OrderDetails orderDetails, Email email)
    {
        return new OrderItem
        {
            Id = new OrderId(Guid.NewGuid()),
            OrderDetails = orderDetails,
        };
    }
}
