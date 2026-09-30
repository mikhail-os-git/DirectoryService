using DirectoryService.Application.Validation;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.DetachPosition;

public class DetachPositionValidator : AbstractValidator<DetachPositionCommand>
{
    public DetachPositionValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithError(Failure.Validation("Department id can't be empty", "department-id.invalid.empty"));
        
        RuleFor(x => x.PositionId)
            .NotEmpty()
            .WithError(Failure.Validation("Position id can't be empty", "position-id.invalid.empty"));

        When(x => x.PositionId != Guid.Empty && x.DepartmentId != Guid.Empty, () =>
        {
            RuleFor(x => x.DepartmentId)
                .Must((x, departmentId) => x.PositionId != departmentId)
                .WithError(Failure.Validation(
                    "Department id and position id must be different",
                    "department-position.invalid.same-id"));
        });
    }
}