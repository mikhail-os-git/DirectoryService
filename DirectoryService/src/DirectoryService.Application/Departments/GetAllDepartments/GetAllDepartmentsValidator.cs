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
            .WithError(Failure.Validation(
                "Page settings required",
                "page.settings.null"));
        
        RuleFor(x => x.PageSettings.Page)
            .NotNull()
            .WithError(Failure.Validation(
                "Page number must be not null",
                "page.number.null"))
            .When(x => x.PageSettings is not null)
            .GreaterThan(0)
            .WithError(Failure.Validation(
                "Page number must be greater than 0",
                "page.number.negative"));
        
        RuleFor(x => x.PageSettings.PageSize)
            .NotNull()
            .WithError(Failure.Validation(
                "Page Size number must be not null",
                "page-size.number.null"))
            .When(x => x.PageSettings is not null)
            .GreaterThan(0)
            .WithError(Failure.Validation(
                "Page Size number must be greater than 0",
                "page-size.number.negative"))
            .LessThanOrEqualTo(100)
            .WithError(Failure.Validation(
                "Page size must be between 1 and 100", 
                "page-size.number.negative"));
        
        RuleFor(x => x.Search)
            .MaximumLength(500)
            .WithError(Failure.Validation(
                "Search query cannot exceed 500 characters",
                "search.query.too-long")); 
        
        RuleFor(x => x.SortBy)
            .Must(name => allowSortName.Contains(name))
            .WithError(Failure.Validation(
                $"Invalid sortBy value. Allowed values: {string.Join(", ", allowSortName)}",
                "department.sort-by.invalid"));
        
        RuleFor(x => x.SortDir)
            .Must(dir => allowSortDir.Contains(dir))
            .WithError(Failure.Validation(
                $"Invalid sort-direction value. Allowed values: {string.Join(", ", allowSortDir)}",
                "department.sort-dir.invalid"));
    }
}