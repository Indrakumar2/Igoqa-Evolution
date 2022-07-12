using Evolution.Company.Domain.Interfaces.Data;
using Evolution.Company.Domain.Interfaces.Validations;
using Evolution.Company.Infrastructe.Validations;
using Evolution.Company.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Company.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            //Repository
            RegisterCompanyService(services);
            services.AddScoped<ICompanyAddressRepository, CompanyOfficeRepository>();
            services.AddScoped<ICompanyPayrollRepository, CompanyPayrollRepository>();
            services.AddScoped<ICompanyPayrollPeriodRepository, CompanyPayrollPeriodRepository>();
            services.AddScoped<ICompanyInvoiceRepository, CompanyInvoiceRepository>();
            services.AddScoped<ICompanyDivisionRepository, CompanyDivisionRepository>();
            services.AddScoped<ICompanyCostCenterRepository, CompanyDivisionCOCRepository>();
            //services.AddScoped<ICompanyDocumentRepository, CompanyDocumentRepository>();
            services.AddScoped<ICompanyNoteRepository, CompanyNoteRepository>();
            services.AddScoped<ICompanyExpectedMarginRepository, CompanyExpectedMarginRepository>();
            services.AddScoped<ICompanyQualificationRepository, CompanyQualificationRepository>();
            services.AddScoped<ICompanyTaxRepository, CompanyTaxRepository>();
            services.AddScoped<ICompanyEmailTemplateRepository, CompanyEmailTemplateRepository>();

            //ValidationService
            services.AddScoped<ICompanyTaxValidationService, CompanyTaxValidationService>();
           
            services.AddScoped<ICompanyOfficeValidationService, CompanyOfficeValidationService>();
            services.AddScoped<ICompanyPayrollValidationService, CompanyPayrollValidationService>();
            services.AddScoped<ICompanyPayrollPeriodValidationService, CompanyPayrollPeriodValidationService>();
            services.AddScoped<ICompanyInvoiceTextValidationService, CompanyInvoiceValidationService>();
            services.AddScoped<ICompanyDivisionValidationService, CompanyDivisionValidationService>();
            services.AddScoped<ICompanyCostCenterValidationService, CompanyCostCenterValidationService>();
           // services.AddScoped<ICompanyDocumentValidationService, CompanyDocumentValidationService>();
            services.AddScoped<ICompanyNoteValidationService, CompanyNoteValidationService>();
            services.AddScoped<ICompanyExpectedMarginValidationService, CompanyExpectedMarginValidationService>();
            services.AddScoped<ICompanyQualificationValidationService, CompanyQualificationValidationService>();
        }

        public void RegisterCompanyService(IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyValidationService, CompanyValidationService>();
        }
    }
}
