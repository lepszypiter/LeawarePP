namespace LeawareTest.BuildingBlocks;

public abstract class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;

    protected UnitOfWork(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
