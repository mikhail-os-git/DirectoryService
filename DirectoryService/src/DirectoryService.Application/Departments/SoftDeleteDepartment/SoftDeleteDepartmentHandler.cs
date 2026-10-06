using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using General.Errors;

namespace DirectoryService.Application.Departments.SoftDeleteDepartment;

public class SoftDeleteDepartmentHandler: ICommandHandler<Guid, SoftDeleteDepartmentCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;

    public SoftDeleteDepartmentHandler(IDepartmentsRepository departmentsRepository, ITransactionManager transactionManager)
    {
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, FailList>> Handle(
        SoftDeleteDepartmentCommand command,
        CancellationToken cancellationToken)
    {
        if (command.DepartmentId == Guid.Empty)
            return Failure.Validation("Department Id can't be Empty", "department-id.invalid.empty").ToFailList();

        Department? department =
            await _departmentsRepository.GetByAsync(d => d.Id == command.DepartmentId, cancellationToken);

        if (department is null)
            return DepartmentErrors.NotFound(command.DepartmentId).ToFailList();

        bool hasChildren = await _departmentsRepository.HasDescendantsAsync(department.Path, cancellationToken);

        if (hasChildren)
        {
            return Failure.Conflict(
                $"Department with id '{command.DepartmentId}' has active child departments and cannot be deleted",
                "department.has.children").ToFailList();
            
        }
        
        department.Delete();

        var save = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (save.IsFailure)
            return save.Error.ToFailList();

        return command.DepartmentId;
    }
}