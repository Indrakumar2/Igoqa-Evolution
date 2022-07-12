//using Evolution.Common.Interfaces.Services;
using Evolution.Company.Core.Services;
using Evolution.Company.Domain.Interfaces.Companies;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Company.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterService(services);
        }
        
        public void RegisterService(IServiceCollection services)
        {          
            services.AddScoped<ICompanyInvoiceService, CompanyInvoiceService>();
            services.AddScoped<ICompanyDivisionService, CompanyDivisionService>();
            RegisterCompanyService(services);
            services.AddScoped<ICompanyOfficeService, CompanyOfficeService>();            
            services.AddScoped<ICompanyPayrollService, CompanyPayrollService>();            
            services.AddScoped<ICompanyPayrollPeriodService, CompanyPayrollPeriodService>();
            services.AddScoped<ICompanyDivisionCostCenterService, CompanyCostCenterService>(); 
            services.AddScoped<ICompanyNoteService, CompanyNoteService>();
            services.AddScoped<ICompanyExpectedMarginService, CompanyExpectedMarginService>();
            services.AddScoped<ICompanyQualificationService, CompanyQualificationService>();            
            services.AddScoped<ICompanyTaxService, CompanyTaxService>();
            services.AddScoped<ICompanyEmailTemplateService, CompanyEmailTemplateService>();
            services.AddScoped<ICompanyDetailService, CompanyDetailService>();            
        }

        public void RegisterCompanyService(IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
        }
    }
}
