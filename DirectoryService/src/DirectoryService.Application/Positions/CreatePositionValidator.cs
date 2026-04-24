using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Positions;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Positions;

public class CreatePositionValidator: AbstractValidator<CreatePositionRequest>
{
    public CreatePositionValidator()
    {
        RuleFor(p => p.Name).MustBeValueObject(PositionName.Create);
        RuleFor(p => p.DepartmentIds)
            .NotEmpty()
            .WithError(Failure.Validation("The collection should not be empty.", "department-id.collection.invalid"))
            .AllUnique()
            .WithError(Failure.Validation("All items in the collection must be unique.", "department-id.collection.invalid"));

        RuleFor(p => p.Description).MaximumLength(LengthConstants.MAX_LENGTH_1000).WithError(
            Failure.Validation(
                $"the description text is too long, the maximum number of characters: {LengthConstants.MAX_LENGTH_1000}",
                "position.description.is.invalid"));
    }
}