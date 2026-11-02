using DirectoryService.Contracts.Departments.CreateDepartment;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Shared.Core.Validation;
using Shared.Kernal.Failures;

namespace DirectoryService.Application.Departments.CreateDepartment;

public class CreateDepartmentValidator: AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentValidator()
    {
        RuleFor(d => d.Name).MustBeValueObject(DepartmentName.Create);

        RuleFor(d => d.Identifier).MustBeValueObject(Identifier.Create);

        RuleFor(d => d.LocationIds)
            .NotEmpty()
            .WithError(CommonErrors.CollectionEmpty("location-id"))
            .AllUnique()
            .WithError(CommonErrors.UniqueCollectionInvalid("location-id"));
    }
}