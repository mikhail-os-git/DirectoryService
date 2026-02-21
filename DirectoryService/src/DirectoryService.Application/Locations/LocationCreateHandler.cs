using CSharpFunctionalExtensions;
using DirectoryService.Infrastructure.Locations.Interfaces;
using DirectoryService.Infrastructure.ValueObjects;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure.Locations;

public class LocationCreateHandler : ILocationCreateHandler
{
    private readonly ILocationsRepository _repository;
    private readonly ILogger<LocationCreateHandler> _logger;

    public LocationCreateHandler(ILocationsRepository repository, ILogger<LocationCreateHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, string>> Handle(LocationRequestDto request, CancellationToken cancellationToken = default)
    {
        // Валидпция входных параметров
        
        // Бизнес валидация
        
        // Создание доменных моделей
        var name = LocationName.Create(request.Name);
        if (name.IsFailure)
        {
            _logger.LogError("Validation failed for name: {Error}", name.Error);

            return name.Error;
        }

        var timezone = Timezone.Create(request.Timezone);

        if (timezone.IsFailure)
        {
            _logger.LogError("Validation failed for timezone: {Error}", timezone.Error);
            return timezone.Error;
        }

        var address = Address.Create(
            request.Address.Country,
            request.Address.City,
            request.Address.Street,
            request.Address.HouseNumber,
            request.Address.PostalCode);

        if (address.IsFailure)
        {
            _logger.LogError("Validation failed for address: {Error}", address.Error);
            
            return address.Error;
        }

        var location = Location.Create(name.Value, address.Value, timezone.Value);

        if (location.IsFailure) return location.Error;
        
        // Сохранение доменных моделей в БД
        var result = await _repository.AddAsync(location.Value, cancellationToken);

        if (result.IsFailure) return result.Error;

        return result.Value;
    }
}