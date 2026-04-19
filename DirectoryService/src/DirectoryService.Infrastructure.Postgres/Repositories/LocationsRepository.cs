using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationsRepository: ILocationsRepository
{
    private readonly DirectoryServiceDbContext _context;
    private readonly ILogger<LocationsRepository> _logger;

    public LocationsRepository(DirectoryServiceDbContext context, ILogger<LocationsRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> AddAsync(Location location, CancellationToken cancellationToken)
    {
        await _context.Locations.AddAsync(location, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Location added with Id: {LocationId}", location.Id); 
        
        return location.Id;
    }

    public async Task<bool> LocationNameExistsAsync(LocationName name, CancellationToken cancellationToken)
    {
        bool res = await _context.Locations.AnyAsync(l => l.LocationName == name, cancellationToken);
        return res;
    }
    
    public async Task<bool> LocationAddressExistsAsync(Address address, CancellationToken cancellationToken)
    {
        return await _context.Locations.AnyAsync(
            l => l.Address.Country == address.Country &&
                 l.Address.City == address.City &&
                 l.Address.Street == address.Street &&
                 l.Address.HouseNumber == address.HouseNumber &&
                 l.Address.PostalCode == address.PostalCode,
            cancellationToken);
    }
    
    // public Task<Guid> SaveAsync(Location location, CancellationToken cancellationToken) => throw new NotImplementedException();
    //
    // public Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken) => throw new NotImplementedException();
    //
    // public Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken) => throw new NotImplementedException();
}