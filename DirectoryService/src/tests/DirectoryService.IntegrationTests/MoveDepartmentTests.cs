using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using DirectoryService.TestData.Fixtures;
using General.Errors;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.IntegrationTests;

public class MoveDepartmentTests: DirectoryBaseTests
{
    public MoveDepartmentTests(DirectoryTestWebFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task Move_Department_With_Valid_Data_Expected_Success()
    {
        var sourceRecruiting = DepartmentFixtures.Recruiting;
        var sourceParentIdRecruiting = sourceRecruiting.ParentId;

        var sourceInfraDevOps = DepartmentFixtures.InfraDevOps;
        var sourceSoftwareDev = DepartmentFixtures.SoftwareDev;
        var sourceIt = DepartmentFixtures.IT;
        
        var cnt = CancellationToken.None;
        
        var res1 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(new MoveDepartmentCommand(DepartmentFixtures.RecruitingId, null), cnt));

        var res2 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(new MoveDepartmentCommand(DepartmentFixtures.InfraDevOpsId, DepartmentFixtures.ItId), cnt));
        
        Assert.True(res1.IsSuccess);
        Assert.NotNull(sourceParentIdRecruiting);
        
        Assert.True(res2.IsSuccess);
        Assert.Equal(sourceInfraDevOps.ParentId, sourceSoftwareDev.Id);
        
        var recruiting = await ExecuteInDb(async context =>
                await context.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentFixtures.RecruitingId, cnt));
        
        Assert.NotEqual(sourceRecruiting.Path, recruiting?.Path);
        Assert.Equal(recruiting?.Path.Value, recruiting?.Identifier.Value);
        Assert.NotEqual(recruiting?.ParentId, sourceParentIdRecruiting);
        Assert.Null(recruiting?.ParentId);
        
        var devops = await ExecuteInDb(async context => await context.Departments.FirstOrDefaultAsync(d => d.Id == DepartmentFixtures.InfraDevOpsId, cnt));
        
        Assert.NotEqual(devops?.Path, sourceInfraDevOps.Path);
        Assert.NotEqual(devops?.ParentId, sourceSoftwareDev.Id);
        Assert.Equal(sourceIt.Id, devops?.ParentId);
        Assert.Equal(devops?.Path.Value, $"{sourceIt.Path.Value}.{devops?.Identifier.Value}");
    }
    
    [Fact]
    public async Task Move_Department_With_Invalid_Data_Expected_Failure()
    {
        var command1 = new MoveDepartmentCommand(Guid.NewGuid(), null);
        var command2 = new MoveDepartmentCommand(DepartmentFixtures.RecruitingId, Guid.NewGuid());
        var command3 = new MoveDepartmentCommand(DepartmentFixtures.HrId, DepartmentFixtures.RecruitingId);
        var command4 = new MoveDepartmentCommand(DepartmentFixtures.HrId, DepartmentFixtures.HrId);
        
        var cnt = CancellationToken.None;
        var conflictFailure = Failure
            .Conflict("A descendant department cannot be set as a parent", "department.path.conflict")
            .ToFailList();
        var validationFailure =
            Failure.Validation("Department cannot be moved to itself", "department.request.invalid");
        
        var res1 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(command1, cnt));
        var res2 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(command2, cnt));
        var res3 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(command3, cnt));
        var res4 = await ExecuteHandler<MoveDepartmentHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(command4, cnt));
        
        Assert.True(res1.IsFailure && res2.IsFailure && res3.IsFailure && res4.IsFailure);
        Assert.Equal(res1.Error, DepartmentErrors.NotFound(command1.departmentId).ToFailList());
        Assert.Equal(res2.Error, DepartmentErrors.NotFound(command2.parentId!.Value).ToFailList());
        Assert.Equal(res3.Error, conflictFailure);
        Assert.Equal(res4.Error, validationFailure.ToFailList());
    }
}