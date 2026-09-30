using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DirectoryService.Infrastructure.Configurations;

public static class DbContextExtensions
{
    public static DbTransaction? GetCurrentTransaction(this DbContext dbContext) =>
        dbContext.Database.CurrentTransaction?.GetDbTransaction();
}