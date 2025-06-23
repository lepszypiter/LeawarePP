using LeawareTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeawareTest.Infrastructure.EntityConfiguration;

public class EmailsEntityTypeConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("Emails");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
                id => id.Value,
                value => new EmailId(value))
            .IsRequired();

        builder.Property(e => e.ExternalMessageId)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(e => e.ElmContent)
            .IsRequired()
            .HasColumnType("LONGTEXT");

        builder.Property(e => e.Body)
            .IsRequired()
            .HasColumnType("LONGTEXT");
    }
}
