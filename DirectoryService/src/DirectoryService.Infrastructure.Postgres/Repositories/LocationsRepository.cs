using CSharpFunctionalExtensions;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using General.Errors;
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

    public async Task<Result<Guid, Failure>> AddAsync(Location location, CancellationToken cancellationToken)
    {
      await _context.Locations.AddAsync(location, cancellationToken);
      var result = await SaveAsync(cancellationToken);

      if (result.IsFailure)
          return result.Error;
      
      _logger.LogInformation("Location {Id} created successfully", location.Id);
      
      return location.Id;
    }

    public async Task<bool> LocationNameExistsAsync(LocationName name, CancellationToken cancellationToken)
    {
        return await _context.Locations.AnyAsync(l => l.LocationName == name, cancellationToken);
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

    public async Task<bool> AllLocationsExistAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        List<Guid> collection = ids.ToList();
        int foundCount = await _context.Locations
            .CountAsync(l => collection.Contains(l.Id), cancellationToken);

        return foundCount == collection.Count;
    }

    public async Task<UnitResult<Failure>> SaveAsync(CancellationToken cancellationToken) 
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Failure>();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save changes: {Error}", ex);
            return UnitResult.Failure(Failure.Error("Something went wrong", "server.internal"));
        }
    }
    
    // public Task<Guid> DeleteAsync(Guid locationId, CancellationToken cancellationToken) => throw new NotImplementedException();
    //
    // public Task<Guid> GetByIdAsync(Guid locationId, CancellationToken cancellationToken) => throw new NotImplementedException();
}