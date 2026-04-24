using System.ComponentModel;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
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
            .WithError(Failure.Validation("The collection should not be empty.", "location-id.collection.invalid"))
            .AllUnique()
            .WithError(Failure.Validation("All items in the collection must be unique.", "location-id.collection.invalid"));
    }
}