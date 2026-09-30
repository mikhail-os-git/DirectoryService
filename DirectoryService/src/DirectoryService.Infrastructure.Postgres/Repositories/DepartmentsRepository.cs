using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Departments;
using DirectoryService.Infrastructure.Configurations;
using DirectoryService.Infrastructure.Database;
using General.Errors;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Repositories;

public class DepartmentsRepository: IDepartmentsRepository
{
    private readonly DirectoryServiceDbContext _context;
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<DepartmentsRepository> _logger;

    public DepartmentsRepository(DirectoryServiceDbContext context, IDbConnectionFactory connectionFactory,  ILogger<DepartmentsRepository> logger)
    {
        _context = context;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Department department, CancellationToken cancellationToken)
    {
        await _context.Departments.AddAsync(department, cancellationToken);
        return department.Id;
    }

    public async Task<Department?> GetByAsync(
        Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await _context.Departments.FirstOrDefaultAsync(expression, cancellationToken);
    }
    
    public async Task<Department?> GetByIdWithLockAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FromSql($"SELECT * FROM departments WHERE id = {departmentId} FOR UPDATE")
            .FirstOrDefaultAsync(cancellationToken);
        return department;
    }

    public async Task<UnitResult<Failure>> LockDescendantsAsync(string path, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            string selectLock = """SELECT * FROM departments WHERE path <@ @path::ltree AND path != @path::ltree  FOR UPDATE""";

            await connection.ExecuteAsync(selectLock, new { path });

            return UnitResult.Success<Failure>();

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to select with lock");
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }
    
    public async Task<bool> IsMatchAsync(Expression<Func<Department, bool>> expression, CancellationToken cancellationToken)
    {
        return await _context.Departments.AnyAsync(expression, cancellationToken);
    }

    public async Task<bool> AllMatchAsync(IEnumerable<Guid> ids, Expression<Func<Department, bool>> expression,
        CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();

        int count = await _context.Departments.Where(expression).CountAsync(cancellationToken);
        return collection.Count == count;
    }

    public async Task<bool> IsDescendantOfAsync(string potentialDescendantPath, string ancestorPath, CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
        
        string query =
            """ SELECT EXISTS(SELECT 1 FROM departments WHERE  @descendant::ltree <@ @ancestor::ltree AND  @descendant::ltree != @ancestor::ltree) """;
       
        var param = new { descendant = potentialDescendantPath, ancestor = ancestorPath };
        
        var queryCommand = new CommandDefinition(query, param, cancellationToken: cancellationToken);
        
        var result = await connection.ExecuteScalarAsync<bool>(queryCommand);

        return result;
    }
    
    public async Task<UnitResult<Failure>> DeleteDepartmentLocationsByIdAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _context.DepartmentLocations.Where(dl => dl.DepartmentId == departmentId)
                .ExecuteDeleteAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete");
            return UnitResult.Failure(CommonErrors.InternalError);
        }
    }
    
    public async Task<Result<string?, Failure>> MoveDepartmentAsync(string oldChildPath, string parentPath,
        CancellationToken cancellationToken)
    {
        var connection = _context.Database.GetDbConnection();
        
        try
        {
            string updateSql = """
                               UPDATE departments
                               SET 
                                   path = @parentPath::ltree || subpath(@oldChildPath::ltree, -1),
                                   depth = nlevel(@parentPath::ltree || subpath(@oldChildPath::ltree, -1))
                               WHERE path = @oldChildPath::ltree
                               RETURNING path
                               """;

            string? path = await connection.ExecuteScalarAsync<string>(updateSql, new { oldChildPath, parentPath });
            return path;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to select");
            return CommonErrors.InternalError;
        }
    }
    
    public async Task<UnitResult<Failure>> MoveDescendantsAsync(string oldPath, string newPath,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            
            string updateSql = """
                               UPDATE departments
                               SET path = @newPath::ltree || subpath(path, nlevel(@oldPath::ltree)),
                                   depth = nlevel(@newPath::ltree || subpath(path, nlevel(@oldPath::ltree))) - 1
                               WHERE path <@ @oldPath::ltree AND path != @oldPath::ltree
                               """;
            await connection.ExecuteAsync(updateSql, new { newPath, oldPath });

            return UnitResult.Success<Failure>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to update descendants");
            return CommonErrors.InternalError;
        }
    }

    public async Task<Result<int, Failure>> HardDeleteAsync(Department department, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _context.Database.GetDbConnection();
            
            string updateSql = """
                            UPDATE departments
                            SET path = subpath(path, nlevel(@parent_path::ltree)),
                                depth = depth - 1
                            WHERE path <@ @parent_path::ltree AND path != @parent_path::ltree;

                            UPDATE departments
                            SET parent_id = NULL
                            WHERE parent_id = @parent_id;
                            """;
            
            string deleteSql = """
                            DELETE FROM departments
                            WHERE id = @parent_id;
                            """;
            var parameters = new DynamicParameters();
            parameters.Add("parent_path", department.Path.Value);
            parameters.Add("parent_id", department.Id);

            var update = await connection.ExecuteAsync(updateSql, parameters, _context.GetCurrentTransaction());
            var delete = await connection.ExecuteAsync(deleteSql, new { parent_id = department.Id }, _context.GetCurrentTransaction());
            
            return delete;

        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Fail by Hard Delete from Department: {Id}", department.Id);
            return CommonErrors.InternalError;
        }
    }

    public async Task<bool> CheckAttachedPosition(
        Expression<Func<DepartmentPosition, bool>> expression,
        CancellationToken cancellationToken)
    {
        return await _context.DepartmentPositions.AnyAsync(expression, cancellationToken);
    }

    public async Task AttachPosition(Guid depId, Guid posId, CancellationToken cancellationToken) => 
        await _context.DepartmentPositions.AddAsync(new DepartmentPosition(depId, posId), cancellationToken);

    public async Task<int> DetachPosition(
        Expression<Func<DepartmentPosition, bool>> expression,
        CancellationToken cancellationToken) =>
        await _context.DepartmentPositions.Where(expression).ExecuteDeleteAsync(cancellationToken);

    // public async Task<Guid> AddDepartmentLocationsAsync(
    //     Guid departmentId,
    //     IEnumerable<Guid> locationIds,
    //     CancellationToken cancellationToken)
    // {
    //     var list = locationIds.Select(id => new DepartmentLocation(departmentId, id)).ToList();
    //     await _context.DepartmentLocations.AddRangeAsync(list, cancellationToken);
    //     return departmentId;
    // }
    //
    // public async Task<Guid> AddDepartmentLocationsAsync(
    //     IEnumerable<DepartmentLocation> departmentLocations,
    //     CancellationToken cancellationToken)
    // {
    //     var list = departmentLocations.ToList();
    //     await _context.DepartmentLocations.AddRangeAsync(list, cancellationToken);
    //     return list.First().DepartmentId;
    // }

    // public async Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken)
    // {
    //     try
    //     {
    //         await _context.SaveChangesAsync(cancellationToken);
    //         return UnitResult.Success<Failure>();
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError("Failed to save changes: {Error}", ex);
    //         return UnitResult.Failure<Failure>(CommonFailures.InternalError);
    //     }
    // }

    // public Task<Guid> DeleteAsync(Guid departmentId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
