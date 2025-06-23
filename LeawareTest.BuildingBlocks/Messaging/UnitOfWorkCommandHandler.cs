namespace LeawareTest.BuildingBlocks.Messaging;

public abstract class UnitOfWorkCommandHandler<T, TR> : ICommandHandler<T, TR> where T : ICommand<TR>
{
    private readonly IUnitOfWork _unitOfWork;

    protected UnitOfWorkCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected abstract Task<TR> HandleCommand(T command, CancellationToken cancellationToken);

    public async Task<TR> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await HandleCommand(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}

public abstract class UnitOfWorkCommandHandler<T> : ICommandHandler<T> where T : ICommand
{
    private readonly IUnitOfWork _unitOfWork;

    protected UnitOfWorkCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected abstract Task HandleCommand(T command, CancellationToken cancellationToken);

    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        await HandleCommand(request, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
