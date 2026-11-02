using CSharpFunctionalExtensions;
using Shared.Kernel.Failures;

namespace Shared.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery> 
    where TQuery : IQuery
{
    Task<Result<TResponse, FailList>> Handle(TQuery query, CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery>
    where TQuery : IQuery
{
    Task<UnitResult<FailList>> Handle(TQuery query, CancellationToken cancellationToken);
}