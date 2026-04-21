using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Locations;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations;

public class CreateLocationHandler: ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _repository;
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly IValidator<CreateLocationRequest> _validator;

    public CreateLocationHandler(
        ILocationsRepository repository, 
        ILogger<CreateLocationHandler> logger,
        IValidator<CreateLocationRequest> validator)
    {
        _repository = repository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, FailList>> Handle(CreateLocationCommand command, CancellationToken cancellationToken = default)
    {
        // Валидпция входных параметров
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToFailList();
            _logger.LogError("Validation failed: {Errors}", errors);
            return errors;
        }
        
        // Создание доменных моделей
        var name = LocationName.Create(command.Request.Name);
        var timezone = Timezone.Create(command.Request.Timezone);
        
        var address = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.HouseNumber,
            command.Request.Address.PostalCode);
        
        // Бизнес валидация
        if (await _repository.LocationNameExistsAsync(name.Value, cancellationToken))
        {
            return Failure.Conflict("Location with this name already exists", "location-name.conflict").ToFailList();
        }

        if (await _repository.LocationAddressExistsAsync(address.Value, cancellationToken))
        {
            return Failure.Conflict("Location with this address already exists", "location-address.conflict").ToFailList();
        }
        
        var location = Location.Create(name.Value, address.Value, timezone.Value);
        
        // Сохранение доменных моделей в БД
        var id = await _repository.AddAsync(location.Value, cancellationToken);
        
        _logger.LogInformation("Location {Id} created successfully", id);
        
        return id;
    }
}