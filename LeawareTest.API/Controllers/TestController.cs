using LeawareTest.Application;
using LeawareTest.Application.OrderQuery;
using LeawareTest.Application.ProcessEmails;
using LeawareTest.Application.ReadEmail;
using LeawareTest.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LeawareTest.API.Controllers;


[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ISender _sender;

    public TestController(ILogger<TestController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost("check-email")]
    public async Task CheckEmail(CancellationToken cancellationToken)
    {
        _logger.LogInformation("POST: {Name}", nameof(CheckEmail));
        await _sender.Send(new CheckEmailsCommand(), cancellationToken);
    }

    [HttpPost("process-emails")]
    public async Task ProcessEmails(CancellationToken cancellationToken)
    {
        _logger.LogInformation("POST: {Name}", nameof(CheckEmail));
        await _sender.Send(new ProcessEmailsCommand(), cancellationToken);
    }
}
