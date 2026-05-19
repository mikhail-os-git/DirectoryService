using System.Data;
using CSharpFunctionalExtensions;
using General.Errors;

namespace DirectoryService.Application.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Failure>> BeginTransactionAsync(CancellationToken cancellationToken, System.Data.IsolationLevel? isolationLevel = null);
    Task<UnitResult<Failure>> SaveChangesAsync(CancellationToken cancellationToken);
}