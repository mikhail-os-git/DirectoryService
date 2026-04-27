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
            
            // _logger.LogError("Validation failed: {Errors}", errors);
            return errors;
        }
        
        var name = LocationName.Create(command.Request.Name);
        var timezone = Timezone.Create(command.Request.Timezone);
        
        var address = Address.Create(
            command.Request.Address.Country,
            command.Request.Address.City,
            command.Request.Address.Street,
            command.Request.Address.HouseNumber,
            command.Request.Address.PostalCode);
        
        // Бизнес валидация
        bool nameExist = await _repository.IsMatchAsync(l => l.LocationName == name.Value, cancellationToken);
        
        if (nameExist)
            return Failure.Conflict($"Location with this Name already exists : {name.Value.Value}", "location-name.conflict").ToFailList();
        
        bool addressExist = await _repository.IsMatchAsync(
            l => l.Address.Country == address.Value.Country &&
                 l.Address.City == address.Value.City &&
                 l.Address.Street == address.Value.Street &&
                 l.Address.HouseNumber == address.Value.HouseNumber &&
                 l.Address.PostalCode == address.Value.PostalCode,
            cancellationToken);
        
        if (addressExist)
            return Failure.Conflict($"Location with this address already exists : {address.Value}", "location-address.conflict").ToFailList();
        
        // Coздание доменных моделей
        var location = Location.Create(name.Value, address.Value, timezone.Value);
        
        if (location.IsFailure)
            return location.Error.ToFailList();
        
        // Сохранение доменных моделей в БД
        var adding = await _repository.AddAsync(location.Value, cancellationToken);
        if (adding.IsFailure)
            return adding.Error.ToFailList();
        
        return adding.Value;
    }
}