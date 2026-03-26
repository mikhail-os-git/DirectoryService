using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using General;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations;

public class CreateLocationHandler: ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _repository;
    private readonly ILogger<CreateLocationHandler> _logger;

    public CreateLocationHandler(ILocationsRepository repository, ILogger<CreateLocationHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, FailList>> Handle(CreateLocationCommand command, CancellationToken cancellationToken = default)
    {
        // Валидпция входных параметров
        
        // Бизнес валидация
        
        // Создание доменных моделей
        var name = LocationName.Create(command.Request.Name);
        if (name.IsFailure)
        {
            _logger.LogError("Validation failed for name: {Error}", name.Error.ToString());
            return name.Error.ToFailList();
        }

        var timezone = Timezone.Create(command.Request.Timezone);

        if (timezone.IsFailure)
        {
            _logger.LogError("Validation failed for timezone: {Error}", timezone.Error);
            return timezone.Error.ToFailList();
        }

        var address = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.HouseNumber,
            command.Request.Address.PostalCode);

        if (address.IsFailure)
        {
            _logger.LogError("Validation failed for address:\n {Error}", address.Error.ToString());
            return address.Error.ToFailList();
        }

        var location = Location.Create(name.Value, address.Value, timezone.Value);

        if (location.IsFailure) 
            return location.Error.ToFailList();
        
        // Сохранение доменных моделей в БД
        var id = await _repository.AddAsync(location.Value, cancellationToken);

        return id;
    }
}