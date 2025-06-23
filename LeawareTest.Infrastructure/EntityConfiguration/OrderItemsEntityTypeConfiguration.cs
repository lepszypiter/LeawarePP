using LeawareTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeawareTest.Infrastructure.EntityConfiguration;

public class OrderItemsEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasConversion(
                id => id.Value,
                value => new OrderId(value))
            .IsRequired();

        builder.OwnsOne(oi => oi.OrderDetails, od =>
        {
            od.Property(o => o.ProductName).IsRequired().HasMaxLength(100);
            od.Property(o => o.Quantity).IsRequired();
            od.Property(o => o.Price).IsRequired().HasColumnType("decimal(18,2)");
        });
    }
}
