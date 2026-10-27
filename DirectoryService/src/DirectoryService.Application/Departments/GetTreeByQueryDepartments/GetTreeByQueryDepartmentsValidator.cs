using DirectoryService.Application.Validation;
using DirectoryService.Domain.Common.Constants;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.GetTreeByQueryDepartments;

public class GetTreeByQueryDepartmentsValidator: AbstractValidator<GetTreeByQueryDepartmentsQuery>
{
    public GetTreeByQueryDepartmentsValidator()
    {
        RuleFor(q => q.Search)
            .NotEmpty()
            .WithError(Failure.Validation("The Search Text can't be empty", "query.text.invalid.empty"))
            .MinimumLength(LengthConstants.MIN_LENGTH_2)
            .WithError(Failure.Validation(
                $"The query text must be longer than {LengthConstants.MIN_LENGTH_2} characters.",
                "query.text.invalid.too-short"))
            .MaximumLength(LengthConstants.MAX_LENGTH_100)
            .WithError(Failure.Validation(
                $"The query text can't be longer than {LengthConstants.MAX_LENGTH_100} characters.",
                "query.text.invalid.too-long"));

    }
}