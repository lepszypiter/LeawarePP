using LeawareTest.BuildingBlocks;
using LeawareTest.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeawareTest.Infrastructure.Data;

public class AppDbContext : DbContext, IDbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Email> Emails => Set<Email>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public AppDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        optionsBuilder.UseMySql(_configuration.GetConnectionString("DefaultConnection"),
            new MySqlServerVersion(new Version(8, 0, 31))
        );
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
