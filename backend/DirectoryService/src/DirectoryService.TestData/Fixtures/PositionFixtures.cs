using DirectoryService.Domain.Departments;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.TestData.Fixtures;

public static class PositionFixtures
{
    // ── IDs ──────────────────────────────────────────────────────────────
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
    public static readonly Guid BackendDeveloperId = Guid.Parse("00000000-0000-0000-0002-000000000011");
    public static readonly Guid FrontendDeveloperId = Guid.Parse("00000000-0000-0000-0002-000000000012");
    public static readonly Guid MobileDeveloperId = Guid.Parse("00000000-0000-0000-0002-000000000013");
    public static readonly Guid QaEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000014");
    public static readonly Guid TestAutomationEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000015");
    public static readonly Guid SiteReliabilityEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000016");
    public static readonly Guid CloudArchitectId = Guid.Parse("00000000-0000-0000-0002-000000000017");
    public static readonly Guid SecurityAnalystId = Guid.Parse("00000000-0000-0000-0002-000000000018");
    public static readonly Guid CisoId = Guid.Parse("00000000-0000-0000-0002-000000000019");
    public static readonly Guid SupportSpecialistId = Guid.Parse("00000000-0000-0000-0002-000000000020");
    public static readonly Guid DataEngineerId = Guid.Parse("00000000-0000-0000-0002-000000000021");
    public static readonly Guid DataAnalystId = Guid.Parse("00000000-0000-0000-0002-000000000022");
    public static readonly Guid TeamLeadId = Guid.Parse("00000000-0000-0000-0002-000000000023");
    public static readonly Guid LearningSpecialistId = Guid.Parse("00000000-0000-0000-0002-000000000024");
    public static readonly Guid CompensationAnalystId = Guid.Parse("00000000-0000-0000-0002-000000000025");
    public static readonly Guid EmployeeRelationsSpecialistId = Guid.Parse("00000000-0000-0000-0002-000000000026");
    public static readonly Guid AccountsPayableClerkId = Guid.Parse("00000000-0000-0000-0002-000000000027");
    public static readonly Guid AccountsReceivableClerkId = Guid.Parse("00000000-0000-0000-0002-000000000028");
    public static readonly Guid ControllerId = Guid.Parse("00000000-0000-0000-0002-000000000029");
    public static readonly Guid TreasuryManagerId = Guid.Parse("00000000-0000-0000-0002-000000000030");
    public static readonly Guid TaxAdvisorId = Guid.Parse("00000000-0000-0000-0002-000000000031");
    public static readonly Guid AccountExecutiveId = Guid.Parse("00000000-0000-0000-0002-000000000032");
    public static readonly Guid SalesOperationsAnalystId = Guid.Parse("00000000-0000-0000-0002-000000000033");
    public static readonly Guid PartnerManagerId = Guid.Parse("00000000-0000-0000-0002-000000000034");
    public static readonly Guid ContentWriterId = Guid.Parse("00000000-0000-0000-0002-000000000035");
    public static readonly Guid PerformanceMarketingManagerId = Guid.Parse("00000000-0000-0000-0002-000000000036");
    public static readonly Guid BrandDesignerId = Guid.Parse("00000000-0000-0000-0002-000000000037");
    public static readonly Guid ComplianceOfficerId = Guid.Parse("00000000-0000-0000-0002-000000000038");
    public static readonly Guid DataProtectionOfficerId = Guid.Parse("00000000-0000-0000-0002-000000000039");
    public static readonly Guid CustomerSuccessManagerId = Guid.Parse("00000000-0000-0000-0002-000000000040");
    public static readonly Guid FacilityManagerId = Guid.Parse("00000000-0000-0000-0002-000000000041");
    public static readonly Guid ProcurementSpecialistId = Guid.Parse("00000000-0000-0000-0002-000000000042");
    public static readonly Guid ProductManagerId = Guid.Parse("00000000-0000-0000-0002-000000000043");
    public static readonly Guid UxDesignerId = Guid.Parse("00000000-0000-0000-0002-000000000044");
    public static readonly Guid ResearchScientistId = Guid.Parse("00000000-0000-0000-0002-000000000045");
    public static readonly Guid InternId = Guid.Parse("00000000-0000-0000-0002-000000000046");
    public static readonly Guid WorkingStudentId = Guid.Parse("00000000-0000-0000-0002-000000000047");
    public static readonly Guid OfficeAssistantId = Guid.Parse("00000000-0000-0000-0002-000000000048");
    public static readonly Guid ScrumMasterId = Guid.Parse("00000000-0000-0000-0002-000000000049");
    public static readonly Guid TechnicalWriterId = Guid.Parse("00000000-0000-0000-0002-000000000050");

    // ── IT ───────────────────────────────────────────────────────────────
    public static readonly Position ItDirector = Create("IT Director", "Oversees all IT operations and strategy.", ItDirectorId,
        DepartmentFixtures.ItId);

    public static readonly Position SoftwareEngineer = Create("Software Engineer", "Designs and develops software systems.", SoftwareEngineerId,
        DepartmentFixtures.SoftwareDevId);

    public static readonly Position DevOpsEngineer = Create("DevOps Engineer", "Manages CI/CD pipelines and cloud infra.", DevOpsEngineerId,
        DepartmentFixtures.InfraDevOpsId);

    public static readonly Position BackendDeveloper = Create("Backend Developer", "Builds APIs and server-side services.", BackendDeveloperId,
        DepartmentFixtures.BackendDevId);

    public static readonly Position FrontendDeveloper = Create("Frontend Developer", "Builds web user interfaces.", FrontendDeveloperId,
        DepartmentFixtures.FrontendDevId);

    public static readonly Position MobileDeveloper = Create("Mobile Developer", "Builds iOS and Android apps.", MobileDeveloperId,
        DepartmentFixtures.MobileDevId);

    public static readonly Position QaEngineer = Create("QA Engineer", "Ensures product quality through testing.", QaEngineerId,
        DepartmentFixtures.QualityAssuranceId, DepartmentFixtures.TestAutomationId);

    public static readonly Position TestAutomationEngineer = Create("Test Automation Engineer", "Writes and maintains automated tests.", TestAutomationEngineerId,
        DepartmentFixtures.TestAutomationId);

    public static readonly Position SiteReliabilityEngineer = Create("Site Reliability Engineer", "Keeps production reliable and observable.", SiteReliabilityEngineerId,
        DepartmentFixtures.CloudPlatformId, DepartmentFixtures.KubernetesId, DepartmentFixtures.ClusterReliabilityId);

    public static readonly Position CloudArchitect = Create("Cloud Architect", "Designs cloud infrastructure.", CloudArchitectId,
        DepartmentFixtures.CloudPlatformId);

    public static readonly Position SecurityAnalyst = Create("Security Analyst", "Monitors and responds to security incidents.", SecurityAnalystId,
        DepartmentFixtures.InfoSecId, DepartmentFixtures.SecurityOpsCenterId);

    public static readonly Position Ciso = Create("CISO", "Owns the information security strategy.", CisoId,
        DepartmentFixtures.InfoSecId);

    public static readonly Position SupportSpecialist = Create("Support Specialist", "Resolves user and customer requests.", SupportSpecialistId,
        DepartmentFixtures.ItSupportId, DepartmentFixtures.HelpdeskId, DepartmentFixtures.CustomerSupportId);

    public static readonly Position DataEngineer = Create("Data Engineer", "Builds and maintains data pipelines.", DataEngineerId,
        DepartmentFixtures.DataEngineeringId);

    public static readonly Position DataAnalyst = Create("Data Analyst", "Turns data into business insights.", DataAnalystId,
        DepartmentFixtures.DataAnalyticsId, DepartmentFixtures.BusinessIntelligenceId, DepartmentFixtures.ControllingId);

    public static readonly Position TeamLead = Create("Team Lead", "Leads a development team.", TeamLeadId,
        DepartmentFixtures.SoftwareDevId, DepartmentFixtures.BackendDevId, DepartmentFixtures.FrontendDevId,
        DepartmentFixtures.MobileDevId, DepartmentFixtures.QualityAssuranceId);

    // ── HR ───────────────────────────────────────────────────────────────
    public static readonly Position HrManager = Create("HR Manager", "Leads HR operations and employee relations.", HrManagerId,
        DepartmentFixtures.HrId);

    public static readonly Position Recruiter = Create("Recruiter", "Sources and coordinates candidate hiring.", RecruiterId,
        DepartmentFixtures.RecruitingId);

    public static readonly Position LearningSpecialist = Create("Learning Specialist", "Runs trainings and development programs.", LearningSpecialistId,
        DepartmentFixtures.LearningDevelopmentId);

    public static readonly Position CompensationAnalyst = Create("Compensation Analyst", "Designs pay and benefits structures.", CompensationAnalystId,
        DepartmentFixtures.CompensationBenefitsId);

    public static readonly Position EmployeeRelationsSpecialist = Create("Employee Relations Specialist", "Handles workplace relations and conflicts.", EmployeeRelationsSpecialistId,
        DepartmentFixtures.EmployeeRelationsId);

    // ── Finance ──────────────────────────────────────────────────────────
    public static readonly Position Cfo = Create("CFO", "Responsible for financial planning.", CfoId,
        DepartmentFixtures.FinanceId);

    public static readonly Position Accountant = Create("Accountant", "Handles bookkeeping and financial reports.", AccountantId,
        DepartmentFixtures.AccountingId);

    public static readonly Position AccountsPayableClerk = Create("Accounts Payable Clerk", "Processes supplier invoices.", AccountsPayableClerkId,
        DepartmentFixtures.AccountsPayableId);

    public static readonly Position AccountsReceivableClerk = Create("Accounts Receivable Clerk", "Manages incoming payments.", AccountsReceivableClerkId,
        DepartmentFixtures.AccountsReceivableId);

    public static readonly Position Controller = Create("Controller", "Prepares budgets and variance analysis.", ControllerId,
        DepartmentFixtures.ControllingId);

    public static readonly Position TreasuryManager = Create("Treasury Manager", "Manages liquidity and cash flow.", TreasuryManagerId,
        DepartmentFixtures.TreasuryId);

    public static readonly Position TaxAdvisor = Create("Tax Advisor", "Handles tax filings and planning.", TaxAdvisorId,
        DepartmentFixtures.TaxId);

    // ── Sales ────────────────────────────────────────────────────────────
    public static readonly Position SalesManager = Create("Sales Manager", "Drives revenue growth and leads sales team.", SalesManagerId,
        DepartmentFixtures.SalesId);

    public static readonly Position AccountExecutive = Create("Account Executive", "Closes deals with new customers.", AccountExecutiveId,
        DepartmentFixtures.EnterpriseSalesId, DepartmentFixtures.SmbSalesId);

    public static readonly Position SalesOperationsAnalyst = Create("Sales Operations Analyst", "Maintains CRM and sales reporting.", SalesOperationsAnalystId,
        DepartmentFixtures.SalesOperationsId);

    public static readonly Position PartnerManager = Create("Partner Manager", "Develops reseller partnerships.", PartnerManagerId,
        DepartmentFixtures.PartnerChannelId);

    // ── Marketing ────────────────────────────────────────────────────────
    public static readonly Position MarketingSpecialist = Create("Marketing Specialist", "Plans and executes marketing campaigns.", MarketingSpecialistId,
        DepartmentFixtures.MarketingId);

    public static readonly Position ContentWriter = Create("Content Writer", "Writes blog posts and product copy.", ContentWriterId,
        DepartmentFixtures.ContentMarketingId);

    public static readonly Position PerformanceMarketingManager = Create("Performance Marketing Manager", "Runs paid acquisition channels.", PerformanceMarketingManagerId,
        DepartmentFixtures.PerformanceMarketingId);

    public static readonly Position BrandDesigner = Create("Brand Designer", "Creates visual brand assets.", BrandDesignerId,
        DepartmentFixtures.BrandId, DepartmentFixtures.ProductDesignId);

    // ── Legal ────────────────────────────────────────────────────────────
    public static readonly Position LegalCounsel = Create("Legal Counsel", "Advises on contracts and compliance.", LegalCounselId,
        DepartmentFixtures.LegalId);

    public static readonly Position ComplianceOfficer = Create("Compliance Officer", "Ensures regulatory compliance.", ComplianceOfficerId,
        DepartmentFixtures.ComplianceId);

    public static readonly Position DataProtectionOfficer = Create("Data Protection Officer", "Oversees GDPR compliance.", DataProtectionOfficerId,
        DepartmentFixtures.DataProtectionId, DepartmentFixtures.ComplianceId);

    // ── Customer / Operations / Product / R&D ────────────────────────────
    public static readonly Position CustomerSuccessManager = Create("Customer Success Manager", "Drives customer retention.", CustomerSuccessManagerId,
        DepartmentFixtures.CustomerSuccessId);

    public static readonly Position FacilityManager = Create("Facility Manager", "Manages offices and facilities.", FacilityManagerId,
        DepartmentFixtures.FacilityManagementId);

    public static readonly Position ProcurementSpecialist = Create("Procurement Specialist", "Sources vendors and negotiates purchases.", ProcurementSpecialistId,
        DepartmentFixtures.ProcurementId);

    public static readonly Position ProductManager = Create("Product Manager", "Owns product roadmap and priorities.", ProductManagerId,
        DepartmentFixtures.ProductManagementId);

    public static readonly Position UxDesigner = Create("UX Designer", "Designs user flows and interfaces.", UxDesignerId,
        DepartmentFixtures.ProductDesignId);

    public static readonly Position ResearchScientist = Create("Research Scientist", "Explores new technologies and prototypes.", ResearchScientistId,
        DepartmentFixtures.ResearchDevelopmentId);

    // ── Позиции без департаментов ────────────────────────────────────────
    public static readonly Position Intern = Create("Intern", "Temporary internship position.", InternId);
    public static readonly Position WorkingStudent = Create("Working Student", "Part-time position for students.", WorkingStudentId);
    public static readonly Position OfficeAssistant = Create("Office Assistant", "Supports day-to-day office tasks.", OfficeAssistantId);
    public static readonly Position ScrumMaster = Create("Scrum Master", "Facilitates agile ceremonies.", ScrumMasterId);
    public static readonly Position TechnicalWriter = Create("Technical Writer", "Writes technical documentation.", TechnicalWriterId);

    // ── Collections ──────────────────────────────────────────────────────
    public static IReadOnlyList<Position> All =>
    [
        ItDirector, SoftwareEngineer, DevOpsEngineer,
        HrManager, Recruiter,
        Cfo, Accountant,
        SalesManager, MarketingSpecialist, LegalCounsel,
        BackendDeveloper, FrontendDeveloper, MobileDeveloper, QaEngineer, TestAutomationEngineer,
        SiteReliabilityEngineer, CloudArchitect, SecurityAnalyst, Ciso, SupportSpecialist,
        DataEngineer, DataAnalyst, TeamLead, LearningSpecialist, CompensationAnalyst,
        EmployeeRelationsSpecialist, AccountsPayableClerk, AccountsReceivableClerk, Controller, TreasuryManager,
        TaxAdvisor, AccountExecutive, SalesOperationsAnalyst, PartnerManager, ContentWriter,
        PerformanceMarketingManager, BrandDesigner, ComplianceOfficer, DataProtectionOfficer, CustomerSuccessManager,
        FacilityManager, ProcurementSpecialist, ProductManager, UxDesigner, ResearchScientist,
        Intern, WorkingStudent, OfficeAssistant, ScrumMaster, TechnicalWriter
    ];

    /// <summary>Позиции, не привязанные ни к одному департаменту.</summary>
    public static IReadOnlyList<Position> WithoutDepartments =>
    [
        Intern, WorkingStudent, OfficeAssistant, ScrumMaster, TechnicalWriter
    ];

    private static Position Create(string name, string description, Guid id, params Guid[] departmentIds) =>
        Position.Create(
            PositionName.Convert(name),
            departmentIds.Select(departmentId => new DepartmentPosition(departmentId, id)).ToList(),
            description,
            id).Value;
}
