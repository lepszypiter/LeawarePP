namespace LeawareTest.Domain;

public record OrderDetails(
    string ProductName,
    int Quantity,
    decimal Price
);
