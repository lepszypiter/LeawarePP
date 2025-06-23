using LeawareTest.Application.Interfaces;
using LeawareTest.BuildingBlocks;
using LeawareTest.Domain;
using LeawareTest.Infrastructure.Data;
using LeawareTest.Infrastructure.Repositories;
using LeawareTest.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeawareTest.Infrastructure;

public static class LeawareTestInfrastructure
{
    public static void RegisterLeawareTestInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<OpenAISettings>(configuration.GetSection("OpenAISettings"));
        services.AddDbContext<AppDbContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IEmailRepostiory, EmailRepostiory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IOrderProcessor, OrderProcessor>();
    }
}
