using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments.SoftDeleteDepartment;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.TestData.Fixtures;
using General.Errors;

namespace DirectoryService.IntegrationTests;

public class DeleteDepartmentTests : DirectoryBaseTests
{
    public DeleteDepartmentTests(DirectoryTestWebFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task Delete_Department_With_Valid_Data_Expected_Success()
    {
        var businessIntelligenceId = DepartmentFixtures.BusinessIntelligenceId;
        CancellationToken ct = CancellationToken.None;
        var delete =
            await ExecuteHandler<SoftDeleteDepartmentHandler, Result<Guid, FailList>>((sut) =>
                sut.Handle(new SoftDeleteDepartmentCommand(businessIntelligenceId), ct));
        
        Assert.True(delete.IsSuccess);
        Assert.Equal(delete.Value, businessIntelligenceId);
    }

    [Fact]
    public async Task Delete_Department_With__Invalid_Data_Expected_Success()
    {
        var itId = DepartmentFixtures.ItId;
        var businessIntelligenceId = DepartmentFixtures.BusinessIntelligenceId;
        
        CancellationToken ct = CancellationToken.None;
        
        var errorConflict = Failure.Conflict(
            $"Department with id '{itId}' has active child departments and cannot be deleted",
            "department.has.children").ToFailList();
        
        var deleteIt =
            await ExecuteHandler<SoftDeleteDepartmentHandler, Result<Guid, FailList>>((sut) =>
                sut.Handle(new SoftDeleteDepartmentCommand(itId), ct));
        
        var deleteBusinessIntelligence =
            await ExecuteHandler<SoftDeleteDepartmentHandler, Result<Guid, FailList>>((sut) =>
                sut.Handle(new SoftDeleteDepartmentCommand(businessIntelligenceId), ct));
        
        var deleteBusinessIntelligence2 =
            await ExecuteHandler<SoftDeleteDepartmentHandler, Result<Guid, FailList>>((sut) =>
                sut.Handle(new SoftDeleteDepartmentCommand(businessIntelligenceId), ct));
        
        Assert.True(deleteIt.IsFailure);
        Assert.Equal(deleteIt.Error, errorConflict);
        Assert.Equal(deleteBusinessIntelligence2.Error, DepartmentErrors.NotFound(businessIntelligenceId).ToFailList());
    }
    
}