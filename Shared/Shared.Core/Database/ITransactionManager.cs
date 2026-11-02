using CSharpFunctionalExtensions;
using Shared.Kernal.Failures;

namespace Shared.Core.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Failure>> BeginTransactionAsync(CancellationToken cancellationToken, System.Data.IsolationLevel? isolationLevel = null);
    Task<UnitResult<Failure>> SaveChangesAsync(CancellationToken cancellationToken);
}