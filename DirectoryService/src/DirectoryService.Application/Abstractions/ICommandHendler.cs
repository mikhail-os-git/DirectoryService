using CSharpFunctionalExtensions;

namespace DirectoryService.Application.Abstractions;

public interface ICommand;
public interface ICommandHandler<TResponse, in TCommand> 
    where TCommand : ICommand
{
    Task<Result<TResponse, string>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> 
    where TCommand : ICommand
{
    Task<UnitResult<string>> Handle(TCommand command, CancellationToken cancellationToken);
}