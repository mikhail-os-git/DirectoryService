using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments.GetDepartmentsTree;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetDepartmentsTree;

public class GetTreeDepartmentsHandler : IQueryHandler<PagedResult<GetTreeDepartmentsItem>, GetTreeDepartmentsQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;

    public GetTreeDepartmentsHandler(IDirectoryReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Result<PagedResult<GetTreeDepartmentsItem>, FailList>> Handle(
        GetTreeDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        var itemsQuery = from d in _readDbContext.DepartmentsQuery
            where d.ParentId == null
            orderby d.DepartmentName, d.CreatedAt descending
            select new GetTreeDepartmentsItem
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName,
                Identifier = d.Identifier,
                Path = d.Path,
                Depth = d.Depth,
                HasChildren = _readDbContext.DepartmentsQuery.Any(dp => dp.ParentId == d.Id)
            };
        
        int totalCount = await itemsQuery.CountAsync(cancellationToken);
        
        var items = await itemsQuery
            .Skip((query.PageSettings.Page - 1) * query.PageSettings.PageSize)
            .Take(query.PageSettings.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GetTreeDepartmentsItem>(items, totalCount, query.PageSettings.Page, query.PageSettings.PageSize);
    }
}