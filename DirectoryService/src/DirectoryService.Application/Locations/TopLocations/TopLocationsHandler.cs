using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Contracts.Locations.TopLocations;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Locations.TopLocations;

public class TopLocationsHandler: IQueryHandler<TopLocationResponse, IQuery>
{
    private readonly IDirectoryReadDbContext _readDbContext;

    public TopLocationsHandler(IDirectoryReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<TopLocationResponse, FailList>> Handle(IQuery query, CancellationToken cancellationToken)
    {
        var locations = await (from l in _readDbContext.LocationsQuery
            join dl in _readDbContext.DepartmentLocationsQuery on l.Id equals dl.LocationId
            group dl by l.Id into g
            orderby g.Count() descending
            select new TopLocationItem
            {
                Id = g.Key,
                LocationName = _readDbContext.LocationsQuery
                    .Where(l => l.Id == g.Key)
                    .Select(l => l.LocationName.Value)
                    .FirstOrDefault()!,
                Address = _readDbContext.LocationsQuery
                    .Where(l => l.Id == g.Key)
                    .Select(l => l.Address.ToString())
                    .FirstOrDefault()!,
                DepartmentCount = g.Count()
            }).
            Take(5).ToListAsync(cancellationToken);
        return new TopLocationResponse(locations);
    }
}