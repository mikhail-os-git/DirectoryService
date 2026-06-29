using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.TestData.Fixtures;

public static class PositionFixtures
{
    public static readonly Guid ItDirectorId = Guid.Parse("00000000-0000-0000-0002-000000000001");
    public static readonly Guid SoftwareEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000002");
    public static readonly Guid DevOpsEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000003");
    public static readonly Guid HrManagerId = Guid.Parse("00000000-0000-0000-0002-000000000004");
    public static readonly Guid RecruiterId = Guid.Parse("00000000-0000-0000-0002-000000000005");
    public static readonly Guid CfoId = Guid.Parse("00000000-0000-0000-0002-000000000006");
    public static readonly Guid AccountantId = Guid.Parse("00000000-0000-0000-0002-000000000007");
    public static readonly Guid SalesManagerId = Guid.Parse("00000000-0000-0000-0002-000000000008");
    public static readonly Guid MarketingSpecialistId = Guid.Parse("00000000-0000-0000-0002-000000000009");
    public static readonly Guid LegalCounselId = Guid.Parse("00000000-0000-0000-0002-000000000010");

    public static readonly Position ItDirector = Position.Create(
        PositionName.Convert("IT Director"),
        [new DepartmentPosition(DepartmentFixtures.ItId, ItDirectorId)],
        "Oversees all IT operations and strategy.",
        ItDirectorId).Value;

    public static readonly Position SoftwareEngineer = Position.Create(
        PositionName.Convert("Software Engineer"),
        [new DepartmentPosition(DepartmentFixtures.SoftwareDevId, SoftwareEngineerId)],
        "Designs and develops software systems.",
        SoftwareEngineerId).Value;

    public static readonly Position DevOpsEngineer = Position.Create(
        PositionName.Convert("DevOps Engineer"),
        [new DepartmentPosition(DepartmentFixtures.InfraDevOpsId, DevOpsEngineerId)],
        "Manages CI/CD pipelines and cloud infra.",
        DevOpsEngineerId).Value;

    public static readonly Position HrManager = Position.Create(
        PositionName.Convert("HR Manager"),
        [new DepartmentPosition(DepartmentFixtures.HrId, HrManagerId)],
        "Leads HR operations and employee relations.",
        HrManagerId).Value;

    public static readonly Position Recruiter = Position.Create(
        PositionName.Convert("Recruiter"),
        [new DepartmentPosition(DepartmentFixtures.RecruitingId, RecruiterId)],
        "Sources and coordinates candidate hiring.",
        RecruiterId).Value;

    public static readonly Position Cfo = Position.Create(
        PositionName.Convert("CFO"),
        [new DepartmentPosition(DepartmentFixtures.FinanceId, CfoId)],
        "Responsible for financial planning.",
        CfoId).Value;

    public static readonly Position Accountant = Position.Create(
        PositionName.Convert("Accountant"),
        [new DepartmentPosition(DepartmentFixtures.AccountingId, AccountantId)],
        "Handles bookkeeping and financial reports.",
        AccountantId).Value;

    public static readonly Position SalesManager = Position.Create(
        PositionName.Convert("Sales Manager"),
        [new DepartmentPosition(DepartmentFixtures.SalesId, SalesManagerId)],
        "Drives revenue growth and leads sales team.",
        SalesManagerId).Value;

    public static readonly Position MarketingSpecialist = Position.Create(
        PositionName.Convert("Marketing Specialist"),
        [new DepartmentPosition(DepartmentFixtures.MarketingId, MarketingSpecialistId)],
        "Plans and executes marketing campaigns.",
        MarketingSpecialistId).Value;

    public static readonly Position LegalCounsel = Position.Create(
        PositionName.Convert("Legal Counsel"),
        [new DepartmentPosition(DepartmentFixtures.LegalId, LegalCounselId)],
        "Advises on contracts and compliance.",
        LegalCounselId).Value;

    public static IReadOnlyList<Position> All =>
    [
        ItDirector, SoftwareEngineer, DevOpsEngineer,
        HrManager, Recruiter,
        Cfo, Accountant,
        SalesManager, MarketingSpecialist, LegalCounsel
    ];
}
