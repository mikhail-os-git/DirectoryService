using CSharpFunctionalExtensions;
using General.Errors;

namespace DirectoryService.Application.Database;

public interface ITransactionScope : IAsyncDisposable, IDisposable
{
    Task<UnitResult<Failure>> CommitAsync(CancellationToken cancellationToken);
    Task<UnitResult<Failure>> RollbackAsync(CancellationToken cancellationToken);
}