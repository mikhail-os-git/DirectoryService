using CSharpFunctionalExtensions;
using General.Errors;

namespace DirectoryService.Application.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Failure>> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<UnitResult<Failure>> SaveChangesAsync(CancellationToken cancellationToken);
}