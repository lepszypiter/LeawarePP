namespace LeawareTest.Domain;

public class Email
{
    public EmailId Id { get; private set; } = default!;
    public string ExternalMessageId { get; private set; } = default!;
    public string Subject { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public string ElmContent { get; private set; } = default!;
    public bool IsProcessed { get; private set; }

    public void MarkAsProcessed()
    {
        IsProcessed = true;
    }

    public static Email Create(string ExternalMessageId, string subject, string body, string elmContent)
    {
        return new Email
        {
            Id = new EmailId(Guid.NewGuid()),
            ExternalMessageId = ExternalMessageId,
            Subject = subject,
            Body = body,
            ElmContent = elmContent
        };
    }
}
