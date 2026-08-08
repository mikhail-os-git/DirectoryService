using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Application.Departments.UpdateDepartmentLocations;
using DirectoryService.Domain.Common;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Departments;
using DirectoryService.TestData.Fixtures;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace DirectoryService.IntegrationTests;

public class UpdateDepartmentLocationsTests : DirectoryBaseTests
{
    public UpdateDepartmentLocationsTests(DirectoryTestWebFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task Update_DepartmentLocations_With_Valid_Data_Expected_Success()
    {
        var cnt = CancellationToken.None;
        var res = await ExecuteHandler<UpdateDepartmentLocationsHandler, Result<Guid, FailList>>(sut => sut.Handle(
            new UpdateDepartmentLocationsCommand(DepartmentFixtures.AccountingId, [LocationFixtures.CologneId]),
            cnt));

        Assert.True(res.IsSuccess);
        Assert.Equal(DepartmentFixtures.AccountingId, res.Value);

        var departmentId = res.Value;

        await ExecuteInDb(async context =>
        {
            DepartmentLocation? departmentLocation = await context.DepartmentLocations
                .FirstOrDefaultAsync(
                    dl => dl.DepartmentId == departmentId && dl.LocationId == LocationFixtures.CologneId,
                    cnt);
        
            Assert.NotNull(departmentLocation);
        });
    }

    [Fact]
    public async Task Update_DepartmentLocations_With_Invalid_Data_Expected_Failure()
    {
        var cnt = CancellationToken.None;

        Guid invalidDepartmentId = Guid.NewGuid(); 
        List<Guid> invalidLocationIds = [Guid.NewGuid(), Guid.NewGuid()];
        var res1 = await ExecuteHandler<UpdateDepartmentLocationsHandler, Result<Guid, FailList>>(
            sut => sut.Handle(
                new UpdateDepartmentLocationsCommand(invalidDepartmentId, [LocationFixtures.MunichId, LocationFixtures.AmsterdamId]),
                cnt));

        var res2 = await ExecuteHandler<UpdateDepartmentLocationsHandler, Result<Guid, FailList>>(
            sut => sut.Handle(
            new UpdateDepartmentLocationsCommand(DepartmentFixtures.HrId, invalidLocationIds),
            cnt));

        var res3 = await ExecuteHandler<UpdateDepartmentLocationsHandler, Result<Guid, FailList>>(sut =>
            sut.Handle(
                new UpdateDepartmentLocationsCommand(Guid.Empty, [Guid.Empty, LocationFixtures.ZurichId]),
                cnt));

        FailList errors = new FailList([
            Failure.Validation("The Department ID must not be empty.", "department-id.invalid"),
            CommonErrors.CollectionItemsInvalid("Location-id")
        ]);
        
        Assert.True(res1.IsFailure && res2.IsFailure && res3.IsFailure);
        Assert.Equal(res1.Error, DepartmentErrors.NotFound(invalidDepartmentId).ToFailList());
        Assert.Equal(res2.Error, LocationErrors.CollectionNotFound(invalidLocationIds).ToFailList());
        Assert.Equal(res3.Error, errors);
    }
    
}