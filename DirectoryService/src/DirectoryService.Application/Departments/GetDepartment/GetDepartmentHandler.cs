using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments.GetDepartment;
using DirectoryService.Domain.Common.DomainEntityErrors;
using FluentValidation;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Departments.GetDepartment;

public class GetDepartmentHandler : IQueryHandler<GetDepartmentResponse, GetDepartmentQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IValidator<GetDepartmentQuery> _validator;

    public GetDepartmentHandler(IDirectoryReadDbContext readDbContext, IValidator<GetDepartmentQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }
    
    public async Task<Result<GetDepartmentResponse, FailList>> Handle(
        GetDepartmentQuery query,
        CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(query, cancellationToken);

        if (!validate.IsValid)
            return validate.ToFailList();

        var department =
            await _readDbContext.DepartmentsQuery.FirstOrDefaultAsync(d => d.Id == query.DepartmentId, cancellationToken);

        if (department is null)
            return DepartmentErrors.NotFound(query.DepartmentId).ToFailList();

        return new GetDepartmentResponse
        {
            Id = department.Id,
            ParentId = department.ParentId,
            DepartmentName = department.DepartmentName.Value,
            Identifier = department.Identifier.Value,
            Path = department.Path.Value,
            Depth = department.Depth,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt.ToLocalTime(),
            UpdatedAt = department.UpdatedAt.ToLocalTime()
        };
    }
}