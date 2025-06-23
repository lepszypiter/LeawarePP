namespace LeawareTest.BuildingBlocks.Messaging;

public interface ICommand : IRequest;

public interface ICommand<TResponse> : IRequest<TResponse>;
