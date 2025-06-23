namespace LeawareTest.BuildingBlocks;

public interface IDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
