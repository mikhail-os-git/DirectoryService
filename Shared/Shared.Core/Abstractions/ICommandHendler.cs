using CSharpFunctionalExtensions;
using Shared.Kernal.Failures;

namespace Shared.Core.Abstractions;

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