using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Positions;
using DirectoryService.Contracts.Positions.CreatePosition;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.Constants;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Positions.CreatePosition;

public class CreatePositionValidator: AbstractValidator<CreatePositionRequest>
{
    public CreatePositionValidator()
    {
        RuleFor(p => p.Name).MustBeValueObject(PositionName.Create);
        RuleForEach(r => r.DepartmentIds).NotEqual(Guid.Empty)
            .WithError(CommonErrors.CollectionItemsInvalid("Department-id"));
        
        RuleFor(p => p.DepartmentIds)
            .NotEmpty()
            .WithError(CommonErrors.CollectionEmpty("department-id"))
            .AllUnique()
            .WithError(CommonErrors.UniqueCollectionInvalid("department-id"));

        RuleFor(p => p.Description).MaximumLength(LengthConstants.MAX_LENGTH_1000).WithError(
            Failure.Validation(
                $"the description text is too long, the maximum number of characters: {LengthConstants.MAX_LENGTH_1000}",
                "position.description.is.invalid"));
    }
}