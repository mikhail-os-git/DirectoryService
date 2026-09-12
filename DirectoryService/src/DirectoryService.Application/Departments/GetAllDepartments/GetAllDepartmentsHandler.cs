using System.Globalization;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Common;
using DirectoryService.Contracts.Departments.GetAllDepartments;
using DirectoryService.Contracts.Departments.GetDepartment;
using DirectoryService.Domain.Departments;
using FluentValidation;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetAllDepartments;

public class GetAllDepartmentsHandler : IQueryHandler<PagedResult<GetAllDepartmentsItem>, GetAllDepartmentsQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IValidator<GetAllDepartmentsRequest> _validator;
    
    public GetAllDepartmentsHandler(IDirectoryReadDbContext readDbContext, IValidator<GetAllDepartmentsRequest> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }
    
    public async Task<Result<PagedResult<GetAllDepartmentsItem>, FailList>> Handle(
        GetAllDepartmentsQuery query,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(query.Request, cancellationToken);

        if (!validation.IsValid)
            return validation.ToFailList();

        var departments = _readDbContext.DepartmentsQuery;

        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            var searchPattern = $"%{query.Request.Search}%";

            departments = departments.Where(
                d => EF.Functions.Like(d.DepartmentName, searchPattern));
        }
        
        departments = query.Request.SortBy switch
        {
            "createdAt" => query.Request.SortDir == "desc" 
                ? departments.OrderByDescending(d => d.CreatedAt) 
                : departments.OrderBy(d => d.CreatedAt),
            _ => query.Request.SortDir == "desc" 
                ? departments.OrderByDescending(d => d.DepartmentName) 
                : departments.OrderBy(d => d.DepartmentName),
        };

        var count = await departments.CountAsync(cancellationToken);
        
        var items = await departments.Select(d => new GetAllDepartmentsItem
            {
                Id = d.Id, DepartmentName = d.DepartmentName.Value, Path = d.Path.Value, CreatedAt = d.CreatedAt,
            })
            .Skip((query.Request.PageSettings.Page - 1) * query.Request.PageSettings.PageSize)
            .Take(query.Request.PageSettings.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<GetAllDepartmentsItem>(items, count, query.Request.PageSettings.Page, query.Request.PageSettings.PageSize);
    }
}