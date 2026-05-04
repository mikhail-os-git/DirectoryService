using System.ComponentModel;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments;

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