using FluentValidation;
using Shared.Core.Validation;
using Shared.Kernal.Failures;

namespace DirectoryService.Application.Departments.MoveDepartment;

public class MoveDepartmentValidator: AbstractValidator<MoveDepartmentCommand>
{
    public MoveDepartmentValidator()
    {
        RuleFor(dc => dc.DepartmentId)
            .NotEqual(Guid.Empty)
            .WithError(Failure.Validation("The Department ID must not be empty.", "department-id.invalid"));

        RuleFor(dc => dc.DepartmentId)
            .Must((dc, id) => !dc.ParentId.HasValue || id != dc.ParentId.Value)
            .WithError(Failure.Validation("Department cannot be moved to itself", "department.request.invalid"));
    }
}