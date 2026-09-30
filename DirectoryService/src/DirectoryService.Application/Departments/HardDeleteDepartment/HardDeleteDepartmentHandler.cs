using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Database;
using DirectoryService.Application.Departments.Interfaces;
using DirectoryService.Domain.Common.DomainEntityErrors;
using General.Errors;

namespace DirectoryService.Application.Departments.HardDeleteDepartment;

public class HardDeleteDepartmentHandler: ICommandHandler<Guid, HardDeleteDepartmentCommand>
{
    private readonly IDepartmentsRepository _departmentsRepository;
    private readonly ITransactionManager _transactionManager;

    public HardDeleteDepartmentHandler(IDepartmentsRepository departmentsRepository, ITransactionManager transactionManager)
    {
        _departmentsRepository = departmentsRepository;
        _transactionManager = transactionManager;
    }
    
    public async Task<Result<Guid, FailList>> Handle(HardDeleteDepartmentCommand command, CancellationToken cancellationToken)
    {
        if (command.DepartmentId == Guid.Empty)
            return Failure.Validation("Department Id can't be Empty", "department-id.invalid.empty").ToFailList();
        
         var scope = await _transactionManager.BeginTransactionAsync(cancellationToken);

         if (scope.IsFailure)
             return scope.Error.ToFailList();

         using var transaction = scope.Value;

         var department = await _departmentsRepository.GetByAsync(d => d.Id == command.DepartmentId, cancellationToken);

         if (department is null)
             return DepartmentErrors.NotFound(command.DepartmentId).ToFailList();

         var delete = await _departmentsRepository.HardDeleteAsync(department, cancellationToken);

         if (delete.IsFailure)
         {
             await transaction.RollbackAsync(cancellationToken);
             return delete.Error.ToFailList();
         }

         if (delete.Value == 0)
         {
             await transaction.RollbackAsync(cancellationToken);
             return DepartmentErrors.NotFound(command.DepartmentId).ToFailList();
         }

         var commit = await transaction.CommitAsync(cancellationToken);
         if (commit.IsFailure)
         {
             await transaction.RollbackAsync(cancellationToken);
             return commit.Error.ToFailList();
         }

         return command.DepartmentId;
    }
}