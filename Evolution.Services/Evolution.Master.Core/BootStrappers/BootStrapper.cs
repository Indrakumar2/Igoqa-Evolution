using Evolution.Master.Core.Services;
using Evolution.Master.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Master.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterMasterService(services);

            services.AddScoped<IModuleDocumentTypeService, ModuleDocumentTypeService>();
            services.AddScoped<IAssignmentLifeCycleService, AssignmentLifeCycleService>();
            services.AddScoped<INativeCurrencyService, NativeCurrencyService>();
            services.AddScoped<ITaxTypeService, TaxService>();           
            services.AddScoped<ICostOfSaleService, CostOfSaleService>();
            services.AddScoped<IAssignmentReferenceType, AssignmentReferenceTypeService>();
            services.AddScoped<ISalutation, SalutationService>();
            services.AddScoped<IPayrollTypeService, PayrollTypeService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IExportPrefixService, ExportPrefixService>();
            services.AddScoped<IEmailPlaceholderService, EmailPlaceholderService>();
            services.AddScoped<ICompanyMarginType, CompanyMarginTypeService>();
            services.AddScoped<IInvoicePaymentTermsService, InvoicePaymentTermsService>();
            services.AddScoped<IStandardChargeScheduleService, StandardChargeScheduleService>();
            services.AddScoped<ICompanyChargeSchedule, CompanyChargeSchedule>();
            services.AddScoped<ICompanyChgSchInspectionGroup, CompanyChgSchInspectionGroupService>();
            services.AddScoped<ICompanyChgSchInspGrpInspectionType, CompanyChgSchInspGrpInspectionTypeService>();
            services.AddScoped<ICompanyInspectionTypeChargeRate,CompanyInspectionTypeChargeRateService>();
            services.AddScoped<ICurrencyExchangeRateService, CurrencyExchangeRateService>();
            services.AddScoped<IIndustrySectorService, IndustrySectorService>();
            services.AddScoped<ILogoService, LogoService>();
            services.AddScoped<IManagedServiceTypeService, ManagedServiceTypeService>();
            services.AddScoped<IProfileActionService, ProfileActionService>();
            services.AddScoped<ISubDivisionService, SubDivisionService>();
            services.AddScoped<IProfileStatusService, ProfileStatusService>();
            services.AddScoped<ICommodityService, CommodityService>();
            services.AddScoped<IEmploymentTypeService, EmploymentTypeService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IProjectTypeService, ProjectTypeService>();
            services.AddScoped<ICodeStandardService, CodeStandardService>();
            services.AddScoped<ICertificationsService, CertificationService>();
            services.AddScoped<ITrainingsService, TrainingsService>();
            services.AddScoped<IComputerKnowledgeService, ComputerKnowledgeService>();
            services.AddScoped<ITaxonomyCategoryService, TaxonomyCategoryService>();
            services.AddScoped<ITaxonomySubCategoryService, TaxonomySubCategoryService>();
            services.AddScoped<ITaxonomyServices, TaxonomyServices>();
            services.AddScoped<ITechnicalSpecialistCustomerService, TechnicalSpecilistCustomerService>();
            services.AddScoped<IExpenseType, ExpenseTypeService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ICompetencyService, CompetencyService>();
            services.AddScoped<IInternalTrainingService, InternalTrainingService>();
            services.AddScoped<ICustomerCommodityService, CustomerCommodityService>();
            services.AddScoped<IAssignmentStatusService, AssignmentStatusService>();
            services.AddScoped<IAssignmentTypeService, AssignmentTypeService>();
            services.AddScoped<IVisitStatusService, VisitStatusService>();
            services.AddScoped<IUnusedReasonService, UnusedReasonService>();
            services.AddScoped<IReviewAndModerationService, ReviewAndModerationService>();
            services.AddScoped<ITaxonomyBusinessUnitService, TaxonomyBusinessUnitService>();
            services.AddScoped<ILeaveCategoryTypeService, LeaveCategoryTypeService>();
            services.AddScoped<ITimeOffRequestService, TimeOffRequestCategoryService>(); 
            services.AddScoped<IDispositionTypeService, DispositionTypeService>();
            services.AddScoped<ISupplierPerformanceTypeService, SupplierPerformanceTypeService>();
            services.AddScoped<IDocumentationService, DocumentationService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IModuleTypeService, ModuleTypeService>();
            services.AddScoped<ICommodityEquipmentService, CommodityEquipmentService>(); 
            services.AddScoped<ITechnicalSpecialistStampCountryCodeService, TechnicalSpecialistStampCountryCodeService>(); 

        }

        public void RegisterMasterService(IServiceCollection services)
        {
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
           // services.AddScoped<IMasterAuditSearchService, MasterAuditSearchService>();
        }
    }
}
