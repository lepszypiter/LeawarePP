using LeawareTest.Application.ProcessEmails;
using LeawareTest.Application.ReadEmail;
using MediatR;
using Microsoft.Extensions.Options;

namespace LeawareTest.API.BackgroundServices;

public record EmailCheckSettings
{
    public TimeSpan CheckPeriod { get; init; }
    public bool Enabled { get; init; }
}

internal class EmailCheckService : BackgroundService
{
    private readonly ILogger<EmailCheckService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly EmailCheckSettings _checkSettings;

    public EmailCheckService(ILogger<EmailCheckService> logger, IOptions<EmailCheckSettings> checkSettings, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _checkSettings = checkSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (_checkSettings.Enabled && !stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Periodic task running at: {time}", DateTimeOffset.Now);
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                _logger.LogInformation("Checking email..");
                await sender.Send(new CheckEmailsCommand(), stoppingToken);

                _logger.LogInformation("Checking for new orders...");
                await sender.Send(new ProcessEmailsCommand(), stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while checking emails.");
            }
            await Task.Delay(_checkSettings.CheckPeriod, stoppingToken);
        }
    }
}
