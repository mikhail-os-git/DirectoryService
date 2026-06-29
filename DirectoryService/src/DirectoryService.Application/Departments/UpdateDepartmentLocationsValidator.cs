using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.DomainEntityErrors;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments;

public class UpdateDepartmentLocationsValidator: AbstractValidator<UpdateDepartmentLocationsCommand>
{
    public UpdateDepartmentLocationsValidator()
    {
        RuleFor(c => c).NotNull().WithError(Failure.Error(
                                                          "Invalid Request", "request.invalid"));
        
        RuleFor(c => c.DepartmentId).NotEqual(Guid.Empty)
            .WithError(Failure.Validation("The Department ID must not be empty.", "department-id.invalid"));
        
        RuleForEach(c => c.LocationIds).NotEqual(Guid.Empty)
            .WithError(CommonErrors.CollectionItemsInvalid("Location-id"));
        
        RuleFor(c => c.LocationIds)
            .NotEmpty()
            .WithError(CommonErrors.CollectionEmpty("location-id"))
            .AllUnique()
            .WithError(CommonErrors.UniqueCollectionInvalid("location-id"));
    }
}