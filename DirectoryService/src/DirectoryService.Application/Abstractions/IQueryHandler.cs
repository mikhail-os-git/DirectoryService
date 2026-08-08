using CSharpFunctionalExtensions;
using General.Errors;

namespace DirectoryService.Application.Abstractions;

public interface IQuery;
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