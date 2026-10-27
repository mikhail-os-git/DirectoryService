using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments.GetDapartmentAncestors;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetDepartmentAncestors;

public class GetDepartmentAncestorsHandler : IQueryHandler<PagedResult<GetDepartmentAncestorsItem>, GetDepartmentAncestorsQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IDbConnectionFactory _connectionFactory;

    public GetDepartmentAncestorsHandler(IDirectoryReadDbContext readDbContext, IDbConnectionFactory connectionFactory)
    {
        _readDbContext = readDbContext;
        _connectionFactory = connectionFactory;
    }
    
    public async Task<Result<PagedResult<GetDepartmentAncestorsItem>, FailList>> Handle(
        GetDepartmentAncestorsQuery query,
        CancellationToken cancellationToken)
    {
        if (query.DepartmentId == Guid.Empty)
            return Failure.Validation("Department Id can't be Empty", "department-id.invalid.empty").ToFailList();

        Department? department = await _readDbContext.DepartmentsQuery.FirstOrDefaultAsync(d => d.Id == query.DepartmentId, cancellationToken);
        
        if(department is null) 
            return DepartmentErrors.NotFound(query.DepartmentId).ToFailList();

        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        long? totalCount = null;
        var parameters = new DynamicParameters();
        parameters.Add("child_path", department.Path.Value, DbType.String);
        parameters.Add("offset", (query.PageSettings.Page - 1) * query.PageSettings.PageSize, DbType.Int32);
        parameters.Add("page_size", query.PageSettings.PageSize, DbType.Int32);
        
        string sqlSelect = """
                     SELECT id, name as DepartmentName, parent_id, identifier, path, depth, count(*) over() as total_count
                     FROM departments d
                     WHERE d.path @> @child_path::ltree 
                     AND d.path <> @child_path::ltree
                     AND d.is_active = true
                     ORDER BY d.depth, d.id
                     LIMIT @page_size OFFSET @offset;
                     """;

        var items = await connection.QueryAsync<GetDepartmentAncestorsItem, long, GetDepartmentAncestorsItem>(
            sqlSelect,
            splitOn: "total_count",
            map: (items, count) =>
            {
                if (totalCount is null)
                    totalCount = count;

                return items;
            },
            param: parameters);

        return new PagedResult<GetDepartmentAncestorsItem>(items, totalCount, query.PageSettings.Page,
            query.PageSettings.PageSize);
    }
}