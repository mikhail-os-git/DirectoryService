using CSharpFunctionalExtensions;
using Shared.Kernal.Failures;

namespace Shared.Core.Database;

public interface ITransactionScope : IAsyncDisposable, IDisposable
{
    Task<UnitResult<Failure>> CommitAsync(CancellationToken cancellationToken);
    Task<UnitResult<Failure>> RollbackAsync(CancellationToken cancellationToken);
}