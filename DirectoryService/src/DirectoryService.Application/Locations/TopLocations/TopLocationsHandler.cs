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
        var sql = from g in 
                from dl in _readDbContext.DepartmentLocationsQuery
                group dl by dl.LocationId into g
                select new { LocationId = g.Key, Count = g.Count() }
            join l in _readDbContext.LocationsQuery on g.LocationId equals l.Id
            orderby g.Count descending, g.LocationId ascending 
            select new TopLocationItem
            {
                Id = l.Id,
                LocationName = l.LocationName.Value,
                Address = l.Address.ToString(),
                DepartmentCount = g.Count
            };

        var locations = await sql.Take(5).ToListAsync(cancellationToken);
        return new TopLocationResponse(locations);
    }
}