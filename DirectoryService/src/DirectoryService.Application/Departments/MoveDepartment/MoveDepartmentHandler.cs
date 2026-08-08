using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using FluentValidation;
using General.Errors;

namespace DirectoryService.Application.Departments.MoveDepartment;

public class MoveDepartmentHandler : ICommandHandler<Guid, MoveDepartmentCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly IValidator<MoveDepartmentCommand> _validator;

    public MoveDepartmentHandler(
        IDepartmentsRepository departmentsRepository,
        ITransactionManager transactionManager,
        IValidator<MoveDepartmentCommand> validator)
    {
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
        _validator = validator;
    }
    
    public async Task<Result<Guid, FailList>> Handle(MoveDepartmentCommand command, CancellationToken cancellationToken)
    {
        var validate = await _validator.ValidateAsync(command, cancellationToken);

        // Валидация входных параметоров
        if (!validate.IsValid)
            return validate.ToFailList();

        // создание scope с транзакцией
        var scope = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (scope.IsFailure)
            return scope.Error.ToFailList();
        
        // использование транзакции
        await using var transaction = scope.Value;

        // Бизнес валидация и выгрузка данных
        bool departmentExist =
            await _departmentsRepository.IsMatchAsync(d => d.Id == command.departmentId && d.IsActive, cancellationToken);

        if (!departmentExist)
            return DepartmentErrors.NotFound(command.departmentId).ToFailList();

        Department? department =
            await _departmentsRepository.GetByIdWithLockAsync(command.departmentId, cancellationToken);

        Department? parent = null;
        
        if (command.parentId.HasValue)
        {
            bool checkParent = await _departmentsRepository.IsMatchAsync(d => d.Id == command.parentId.Value && d.IsActive, cancellationToken);

            if (!checkParent)
                return DepartmentErrors.NotFound(command.parentId.Value).ToFailList();

            parent = await _departmentsRepository.GetByIdWithLockAsync(command.parentId.Value, cancellationToken);
        }

        if (parent is not null && department is not null)
        {
            bool isDescendant =
                await _departmentsRepository.IsDescendantOfAsync(parent.Path.Value, department.Path.Value,
                    cancellationToken);
            
            if (isDescendant)
            {
                return Failure
                    .Conflict("A descendant department cannot be set as a parent", "department.path.conflict")
                    .ToFailList();
            }
        }

        // Блокировка наследников
        var lockDescendants = await _departmentsRepository.LockDescendantsAsync(department!.Path.Value, cancellationToken);
        if (lockDescendants.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return lockDescendants.Error.ToFailList();
        }

        // Обновление родителя, пути и глубины
        string oldPath = department.Path.Value;
        string last = oldPath.Split('.').Last();
        var newPath = Domain.ValueObjects.Path.CreateFromString(parent is null ? last : $"{parent.Path.Value}.{last}");
        
        department.SetParent(parent?.Id ?? null);
        department.SetPath(newPath.Value);
        department.SetDepth((short)(newPath.Value.Roads.Count - 1));
        
        var updateDepartment = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (updateDepartment.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return updateDepartment.Error.ToFailList();
        }

        // обновление пути у наследников
        var moveDescendants =
            await _departmentsRepository.MoveDescendantsAsync(oldPath, department.Path.Value, cancellationToken);

        if (moveDescendants.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return moveDescendants.Error.ToFailList();
        }

        // финальный коммит транзакции
        var commit = await transaction.CommitAsync(cancellationToken);
        if (commit.IsFailure)
        {
            await transaction.RollbackAsync(cancellationToken);
            return commit.Error.ToFailList();
        }

        return command.departmentId;

    }
}