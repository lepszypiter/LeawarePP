namespace LeawareTest.Domain;

public interface IEmailRepostiory
{
    Task<IReadOnlyCollection<Email>> GetUnprocessedEmails(CancellationToken cancellationToken);
    void AddNewEmail(Email email);
    public Task<IReadOnlyCollection<string>> GetNonExistingExternalIdsAsync(IReadOnlyCollection<string> externalIds, CancellationToken cancellationToken);
}
