using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Locations.GetLocation;
using DirectoryService.Domain.Common.DomainEntityErrors;
using FluentValidation;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.GetLocation;

public class GetLocationHandler : IQueryHandler<GetLocationResponse, GetLocationQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;
    private readonly IValidator<GetLocationQuery> _validator;

    public GetLocationHandler(IDirectoryReadDbContext readDbContext, IValidator<GetLocationQuery> validator)
    {
        _readDbContext = readDbContext;
        _validator = validator;
    }

    public async Task<Result<GetLocationResponse, FailList>> Handle(
        GetLocationQuery query,
        CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(query, cancellationToken);

        if (!validate.IsValid)
            return validate.ToFailList();
        
        var location =
            await _readDbContext.LocationsQuery.FirstOrDefaultAsync(l => l.Id == query.LocationId, cancellationToken);

        if (location is null)
            return LocationErrors.NotFound(query.LocationId).ToFailList();

        return new GetLocationResponse
        {
            Id = location.Id,
            LocationName = location.LocationName.Value,
            Address = location.Address.ToString(),
            Timezone = location.Timezone.Value,
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt,
            IsActive = location.IsActive
        };
    }
}