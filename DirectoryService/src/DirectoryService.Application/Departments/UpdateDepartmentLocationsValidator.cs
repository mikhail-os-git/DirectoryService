using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Common;
using FluentValidation;

namespace DirectoryService.Application.Departments;

public class UpdateDepartmentLocationsValidator: AbstractValidator<UpdateDepartmentLocationsRequest>
{
    public UpdateDepartmentLocationsValidator()
    {
        RuleFor(r => r.LocationIds)
            .NotEmpty()
            .WithError(CommonErrors.CollectionEmpty("location-id"))
            .AllUnique()
            .WithError(CommonErrors.UniqueCollectionInvalid("location-id"));
    }
}