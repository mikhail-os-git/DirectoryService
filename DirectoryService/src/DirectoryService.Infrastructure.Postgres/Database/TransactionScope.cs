using System.Data;
using System.Data.Common;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Domain.Common;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public sealed class TransactionScope : ITransactionScope
{
    private readonly DbTransaction _transaction;
    private readonly ILogger<TransactionScope> _logger;
    private bool _disposed;

    public TransactionScope(DbTransaction transaction, ILogger<TransactionScope> logger)
    {
        _transaction = transaction;
        _logger = logger;
    }

    public async Task<UnitResult<Failure>> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _transaction.CommitAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to commit transaction");
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }

    public async Task<UnitResult<Failure>> RollbackAsync(CancellationToken cancellationToken)
    {
        try
        {
           await _transaction.RollbackAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to rollback transaction");
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;
        await _transaction.DisposeAsync();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _transaction.Dispose();

    }
}