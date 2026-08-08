using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Locations.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts.Locations;
using DirectoryService.Contracts.Locations.CreateLocation;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using General.Errors;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Application.Locations.CreateLocation;

public class CreateLocationHandler: ICommandHandler<Guid, CreateLocationCommand>
{
    private readonly ILocationsRepository _repository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<CreateLocationHandler> _logger;
    private readonly IValidator<CreateLocationRequest> _validator;

    public CreateLocationHandler(
        ILocationsRepository repository,
        ITransactionManager transactionManager,
        ILogger<CreateLocationHandler> logger,
        IValidator<CreateLocationRequest> validator)
    {
        _repository = repository;
        _transactionManager = transactionManager;
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
            return LocationErrors.PropertyConflict(name.Value.Value, "Name").ToFailList();
        
        bool addressExist = await _repository.IsMatchAsync(
            l => l.Address.Country == address.Value.Country &&
                 l.Address.City == address.Value.City &&
                 l.Address.Street == address.Value.Street &&
                 l.Address.HouseNumber == address.Value.HouseNumber &&
                 l.Address.PostalCode == address.Value.PostalCode,
            cancellationToken);
        
        if (addressExist)
            return LocationErrors.PropertyConflict(address.Error.ToString(), "Address").ToFailList();
        
        // Coздание доменных моделей
        var location = Location.Create(name.Value, address.Value, timezone.Value);
        
        if (location.IsFailure)
            return location.Error.ToFailList();
        
        // Сохранение доменных моделей в БД
        var id = await _repository.AddAsync(location.Value, cancellationToken);
        
        var saving = await _transactionManager.SaveChangesAsync(cancellationToken);
        if (saving.IsFailure)
            return saving.Error.ToFailList();
        
        return id;
    }
}