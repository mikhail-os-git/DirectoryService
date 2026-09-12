using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments.GetAllDepartments;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.GetAllDepartments;

public class GetAllDepartmentsValidator: AbstractValidator<GetAllDepartmentsRequest>
{
    public GetAllDepartmentsValidator()
    {
        string[] allowSortName = ["name", "createdAt"];
        string[] allowSortDir = ["asc", "desc"];

        RuleFor(x => x.PageSettings)
            .NotNull()
            .WithError(Failure.Validation("page.settings.null", "Page settings required"));
        
        RuleFor(x => x.PageSettings.Page)
            .NotNull()
            .WithError(Failure.Validation("page.number.null", "Page number must be not null"))
            .When(x => x.PageSettings is not null)
            .GreaterThan(0)
            .WithError(Failure.Validation("page.number.negative", "Page number must be greater than 0"));
        
        RuleFor(x => x.PageSettings.PageSize)
            .NotNull()
            .WithError(Failure.Validation("page-size.number.null", "Page Size number must be not null"))
            .When(x => x.PageSettings is not null)
            .GreaterThan(0)
            .WithError(Failure.Validation("page-size.number.negative", "Page Size number must be greater than 0"))
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be between 1 and 100");
        
        RuleFor(x => x.Search)
            .MaximumLength(500)
            .WithError(Failure.Validation("search.query.too-long", "Search query cannot exceed 500 characters"));
        
        RuleFor(x => x.SortBy)
            .Must(name => allowSortName.Contains(name))
            .WithError(Failure.Validation(
                "department.sort-by.invalid",
                $"Invalid sortBy value. Allowed values: {string.Join(", ", allowSortName)}"));
        
        RuleFor(x => x.SortDir)
            .Must(dir => allowSortDir.Contains(dir))
            .WithError(Failure.Validation(
                "department.sort-dir.invalid",
                $"Invalid sort-direction value. Allowed values: {string.Join(", ", allowSortDir)}"));
    }
}