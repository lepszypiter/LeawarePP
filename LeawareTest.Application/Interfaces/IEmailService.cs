using LeawareTest.Domain;

namespace LeawareTest.Application.Interfaces;

public record EmailDto(string MessageId, string Subject, string Body, string ElmContent);

public interface IEmailService
{
    Task<IReadOnlyCollection<EmailDto>> ReadEmailAsync(CancellationToken cancellationToken);
}

