using DirectoryService.Application.Validation;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Locations.GetLocation;

public class GetLocationValidator: AbstractValidator<GetLocationQuery>
{
    public GetLocationValidator()
    {
        RuleFor(q => q.LocationId)
            .NotEmpty()
            .WithError(Failure.Validation("The Location Id can not be null", "location-id.invalid"))
            .NotEqual(Guid.Empty)
            .WithError(Failure.Validation("The Location Id must not be empty.", "location-id.invalid"));
    }
}