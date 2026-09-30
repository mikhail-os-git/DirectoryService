using DirectoryService.Application.Validation;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Positions.RenamePosition;

public class RenamePositionValidator : AbstractValidator<RenamePositionCommand>
{
    public RenamePositionValidator()
    {
        RuleFor(x => x.PositionId)
            .NotNull()
            .WithError(Failure.Validation("Position Id can't be null", "position-id.invalid.null"))
            .NotEmpty()
            .WithError(Failure.Validation("Position Id can't be empty", "position-id.invalid.empty"));

        RuleFor(x => x.NewName)
            .NotNull()
            .WithError(Failure.Validation("New position Name can't be null", "position-name.invalid.null"))
            .MustBeValueObject(PositionName.Create);
    }
}