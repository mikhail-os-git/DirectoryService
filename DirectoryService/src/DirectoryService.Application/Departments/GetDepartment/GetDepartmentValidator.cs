using DirectoryService.Application.Validation;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.GetDepartment;

public class GetDepartmentValidator : AbstractValidator<GetDepartmentQuery>
{
    public GetDepartmentValidator()
    {
        RuleFor(q => q.DepartmentId)
            .NotEmpty()
            .WithError(Failure.Validation("The Department Id can not be null", "department-id.invalid"))
            .NotEqual(Guid.Empty)
            .WithError(Failure.Validation("The Department Id must not be empty.", "department-id.invalid"));
    }
}