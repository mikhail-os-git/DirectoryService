using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Common;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments;

public class UpdateDepartmentLocationsValidator: AbstractValidator<UpdateDepartmentLocationsRequest>
{
    public UpdateDepartmentLocationsValidator()
    {
        RuleFor(r => r).NotNull().WithError(Failure.Error(
                                                          "Invalid Request", "request.invalid"));

        RuleForEach(r => r.LocationIds).NotEqual(Guid.Empty)
            .WithError(CommonErrors.CollectionItemsInvalid("Location-id"));
        
        RuleFor(r => r.LocationIds)
            .NotEmpty()
            .WithError(CommonErrors.CollectionEmpty("location-id"))
            .AllUnique()
            .WithError(CommonErrors.UniqueCollectionInvalid("location-id"));
    }
}