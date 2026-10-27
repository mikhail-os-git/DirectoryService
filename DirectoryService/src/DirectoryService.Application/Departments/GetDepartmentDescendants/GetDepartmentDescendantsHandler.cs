using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments.GetDepartmentDescendants;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetDepartmentDescendants;

public class GetDepartmentDescendantsHandler: IQueryHandler<PagedResult<GetDepartmentDescendantsItem>, GetDepartmentDescendantsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly IDirectoryReadDbContext _readDbContext;

    public GetDepartmentDescendantsHandler(IDbConnectionFactory connectionFactory, IDirectoryReadDbContext readDbContext)
    {
        _connectionFactory = connectionFactory;
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<PagedResult<GetDepartmentDescendantsItem>, FailList>> Handle(
        GetDepartmentDescendantsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.DepartmentId == Guid.Empty)
            return Failure.Validation("Department Id can't be Empty", "department-id.invalid.empty").ToFailList();

        Department? department =
            await _readDbContext.DepartmentsQuery.FirstOrDefaultAsync(
                d => d.Id == query.DepartmentId,
                cancellationToken);

        if (department is null)
            return DepartmentErrors.NotFound(query.DepartmentId).ToFailList();
        
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        long? totalCount = null;
        var parameters = new DynamicParameters();
        parameters.Add("root_path", department.Path.Value, DbType.String);
        parameters.Add("offset", (query.PageSettings.Page - 1) * query.PageSettings.PageSize, DbType.Int32);
        parameters.Add("page_size", query.PageSettings.PageSize, DbType.Int32);
        
        string sql = """
                     SELECT id, name as DepartmentName, identifier, path, depth, 
                            (EXISTS(SELECT 1 FROM departments dp WHERE dp.parent_id = d.id AND dp.is_active = true)) as HasChildren,  
                             COUNT(*) OVER() as total_count
                                 from departments d
                     WHERE d.path <@ @root_path::ltree 
                       AND d.path <> @root_path::ltree 
                       AND nlevel(d.path) = nlevel(@root_path::ltree ) + 1
                       AND d.is_active = true
                       LIMIT @page_size OFFSET @offset
                     """;

        var items = await connection.QueryAsync<GetDepartmentDescendantsItem, long, GetDepartmentDescendantsItem>(
            sql, 
            splitOn: "total_count",
            map:
            (item, count) =>
            {
                if (totalCount is null)
                    totalCount = count;
                
                return item;
            },
            param: parameters);

        if (items is null || items.ToList().Count == 0)
            return Failure.NotFoundEntity("No children found", query.DepartmentId).ToFailList();

        return new PagedResult<GetDepartmentDescendantsItem>(items, totalCount, query.PageSettings.Page,
            query.PageSettings.PageSize);

    }
}