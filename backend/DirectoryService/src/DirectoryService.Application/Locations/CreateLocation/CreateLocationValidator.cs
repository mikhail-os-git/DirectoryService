using DirectoryService.Contracts.Locations.CreateLocation;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Shared.Core.Validation;

namespace DirectoryService.Application.Locations.CreateLocation;

public class CreateLocationValidator: AbstractValidator<CreateLocationRequest>
{
    public CreateLocationValidator()
    {
        RuleFor(r => r.Name).MustBeValueObject(LocationName.Create);

        RuleFor(r => r.Timezone).MustBeValueObject(Timezone.Create);

        RuleFor(r => r.Address).MustBeValueObject((address) => Address.Create(
            address.Country,
            address.City,
            address.Street,
            address.HouseNumber,
            address.PostalCode));
    }
}