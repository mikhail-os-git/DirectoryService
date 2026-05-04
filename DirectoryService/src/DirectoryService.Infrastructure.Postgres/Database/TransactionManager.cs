using CSharpFunctionalExtensions;
using DirectoryService.Application.Database;
using DirectoryService.Domain.Common;
using General.Errors;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Database;

public class TransactionManager: ITransactionManager
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<TransactionManager> _logger;

    public TransactionManager(
        DirectoryServiceDbContext dbContext, 
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<TransactionManager>();
    }

    public async Task<Result<ITransactionScope, Failure>> BeginTransactionAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            var scopeLogger = _loggerFactory.CreateLogger<TransactionScope>();
            
#pragma warning disable CA2000
            var scope = new TransactionScope(transaction.GetDbTransaction(), scopeLogger);
#pragma warning restore CA2000

            return scope;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to begin transaction");
            return CommonErrors.InternalError;
        }
    }

    public async Task<UnitResult<Failure>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to save changes");
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }
}