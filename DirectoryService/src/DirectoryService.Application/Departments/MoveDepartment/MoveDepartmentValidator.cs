using DirectoryService.Application.Validation;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.MoveDepartment;

public class MoveDepartmentValidator: AbstractValidator<MoveDepartmentCommand>
{
    public MoveDepartmentValidator()
    {
        RuleFor(dc => dc.departmentId)
            .NotEqual(Guid.Empty)
            .WithError(Failure.Validation("The Department ID must not be empty.", "department-id.invalid"));

        RuleFor(dc => dc.departmentId)
            .Must((dc, id) => !dc.parentId.HasValue || id != dc.parentId.Value)
            .WithError(Failure.Validation("Department cannot be moved to itself", "department.request.invalid"));
    }
}