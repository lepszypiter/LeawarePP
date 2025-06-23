using LeawareTest.Application.Interfaces;
using LeawareTest.Application.ProcessEmails;
using LeawareTest.BuildingBlocks;
using LeawareTest.BuildingBlocks.Messaging;
using LeawareTest.Domain;
using Microsoft.Extensions.Logging;

namespace LeawareTest.Application.ReadEmail;

public record CheckEmailsCommand : ICommand;

public class CheckEmailsCommandHandler : ICommandHandler<CheckEmailsCommand>
{
    private readonly IEmailRepostiory _emailRepostiory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckEmailsCommandHandler> _logger;

    public CheckEmailsCommandHandler(IEmailRepostiory emailRepostiory, IUnitOfWork unitOfWork,
        IEmailService emailService, ILogger<CheckEmailsCommandHandler> logger)
    {
        _emailRepostiory = emailRepostiory;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(CheckEmailsCommand command, CancellationToken cancellationToken)
    {
        var emails = await _emailService.ReadEmailAsync(cancellationToken);
        var receivedIds = emails.Select(email => email.MessageId).ToList();

        var nonExistingIds = await _emailRepostiory.GetNonExistingExternalIdsAsync(receivedIds, cancellationToken);

        foreach (var email in emails.Where(e => nonExistingIds.Contains(e.MessageId)))
        {
            var newEmail = Email.Create(email.MessageId, email.Subject, email.Body, email.ElmContent);
            _emailRepostiory.AddNewEmail(newEmail);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
