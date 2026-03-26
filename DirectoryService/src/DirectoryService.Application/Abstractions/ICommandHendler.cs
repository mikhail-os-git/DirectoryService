using CSharpFunctionalExtensions;
using General;
using General.Errors;

namespace DirectoryService.Application.Abstractions;

public interface ICommand;
public interface ICommandHandler<TResponse, in TCommand> 
    where TCommand : ICommand
{
    Task<Result<TResponse, FailList>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand> 
    where TCommand : ICommand
{
    Task<UnitResult<FailList>> Handle(TCommand command, CancellationToken cancellationToken);
}