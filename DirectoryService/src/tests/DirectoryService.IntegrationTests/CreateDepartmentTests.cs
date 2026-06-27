using CSharpFunctionalExtensions;
using DirectoryService.Application.Departments;
using DirectoryService.Contracts.Departments;
using DirectoryService.Domain.Common.DomainEntityErrors;
using DirectoryService.Domain.Locations;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Infrastructure;
using General.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.IntegrationTests;

public class CreateDepartmentTests: DirectoryBaseTests
{
    public CreateDepartmentTests(DirectoryTestWebFactory factory)
        : base(factory)
    {
    }
    
    [Fact]
    public async Task Create_Department_With_Valid_Data_Expected_Success()
    {
        var locationId = await CreateLocation();
        var ctn = CancellationToken.None;

       var result = await ExecuteHandler<CreateDepartmentHandler, Result<Guid, FailList>>((sut) =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest("тестовое подразделение", "podrazdelenie", [locationId], null));
            return sut.Handle(command, ctn);
        });

        await ExecuteInDb(async context =>
        {
            var department = await context.Departments.FirstOrDefaultAsync(d => d.Id == result.Value, ctn);

            Assert.NotNull(department);
            Assert.Equal(department.Id, result.Value);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });

    }

    [Fact]
    public async Task Create_Department_With_Invalid_Data_Expect_Failure()
    {
        var locationId = Guid.Empty;
        var ctn = CancellationToken.None;
        
        var result = await ExecuteHandler<CreateDepartmentHandler, Result<Guid, FailList>>((sut) =>
        {
            var command = new CreateDepartmentCommand(
                new CreateDepartmentRequest("тестовое подразделение 2", "podraz", [locationId], null));
            return sut.Handle(command, ctn);
        });
        
        Assert.False(result.IsSuccess); 
        Assert.Equal(LocationErrors.CollectionNotFound([locationId]).ToFailList(), result.Error);
    }

    private async Task<Guid> CreateLocation()
    {
        var location = Location.Create(
            LocationName.Create("тестовая локация").Value,
            Address.Create("Россия", "Москва", "Улица ленина", "56А", 143350).Value,
            Timezone.Create("Europe/Moscow").Value).Value;

        var result = await ExecuteInDb(context =>
        {
            context.Locations.Add(location);

            return context.SaveChangesAsync();
        });
        
        return location.Id;
    }
    
}