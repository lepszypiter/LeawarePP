using LeawareTest.Domain;
using LeawareTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LeawareTest.Infrastructure.Repositories;

public class EmailRepostiory : IEmailRepostiory
{
    private readonly AppDbContext _dbContext;

    public EmailRepostiory(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Email>> GetUnprocessedEmails(CancellationToken cancellationToken)
    {
        return await _dbContext.Emails
            .Where(email => !email.IsProcessed)
            .ToListAsync(cancellationToken);
    }

    public void AddNewEmail(Email email)
    {
        _dbContext.Emails.Add(email);
    }
    public async Task<IReadOnlyCollection<string>> GetNonExistingExternalIdsAsync(IReadOnlyCollection<string> externalIds, CancellationToken cancellationToken)
    {
        var existingIds = await _dbContext.Emails
            .Where(email => externalIds.Contains(email.ExternalMessageId))
            .Select(email => email.ExternalMessageId)
            .ToListAsync(cancellationToken);

        return externalIds.Except(existingIds).ToList();
    }
}
