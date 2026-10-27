using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments.GetDepartmentsTree;
using DirectoryService.Domain.Common.DomainEntityErrors;
using FluentValidation;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetTreeByQueryDepartments;

public class
    GetTreeByQueryDepartmentsHandler : IQueryHandler<PagedResult<GetTreeDepartmentsItem>,
    GetTreeByQueryDepartmentsQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IValidator<GetTreeByQueryDepartmentsQuery> _validator;

    public GetTreeByQueryDepartmentsHandler(IDirectoryReadDbContext readDbContext, IValidator<GetTreeByQueryDepartmentsQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }
    
    public async Task<Result<PagedResult<GetTreeDepartmentsItem>, FailList>> Handle(
        GetTreeByQueryDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(query, cancellationToken);
        
        if (!validate.IsValid)
            return validate.ToFailList();
        
        var entities = _readDbContext.DepartmentsQuery;
        var search = from d in entities
            where d.ParentId == null
            where EF.Functions.Like(d.DepartmentName, $"%{query.Search}%")
            orderby d.DepartmentName, d.Id
            select new GetTreeDepartmentsItem
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName,
                Path = d.Path,
                Identifier = d.Identifier,
                Depth = d.Depth,
                HasChildren = entities.Any(dp => dp.ParentId == d.Id)
            };

        long totalCount = await search.CountAsync(cancellationToken);
        var items = await search.ToListAsync(cancellationToken);

        if (items.ToList().Count == 0)
            return DepartmentErrors.CollectionNotFound().ToFailList();

        return new PagedResult<GetTreeDepartmentsItem>(items, totalCount, query.PageSettings.Page,
            query.PageSettings.PageSize);

    }
}