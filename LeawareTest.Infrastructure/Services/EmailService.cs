using System.Text;
using LeawareTest.Application.Interfaces;
using LeawareTest.Domain;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LeawareTest.Infrastructure.Services;
public record EmailSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}

internal class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }

    public async Task<IReadOnlyCollection<EmailDto>> ReadEmailAsync(CancellationToken cancellationToken)
    {
        using var client = new ImapClient();
        try
        {
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect, cancellationToken);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);

            var inbox = client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);


            var emails = new List<EmailDto>();
            for(int index = 0; index< inbox.Count; index++)
            {
                var message = await inbox.GetMessageAsync(index, cancellationToken);
                _logger.LogInformation("Subject: " + message.Subject);
                var elm = ExportAsElm(message);
                emails.Add(new EmailDto(message.MessageId ?? message.Date.ToString("O"), message.Subject, message.HtmlBody ?? message.TextBody, elm));
            }

            return emails.AsReadOnly();
        }
        finally
        {
            await client.DisconnectAsync(true, cancellationToken);
        }
    }

    private string ExportAsElm(MimeMessage message)
    {
        using var stream = new MemoryStream();
        message.WriteTo(stream);
        stream.Position = 0;
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
