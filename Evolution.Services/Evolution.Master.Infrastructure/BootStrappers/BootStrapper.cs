using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Domain.Interfaces.Validations;
using Evolution.Master.Infrastructure.Data;
using Evolution.Master.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterMasterService(services);
            RegisterValidationService(services);
            services.AddScoped<IModuleDocumentTypeRepository, ModuleDocumentTypeRepository>();
            services.AddScoped<ITaxTypeRepository, TaxTypeRepository>();
            services.AddScoped<ICommodityEquipmentRepository, CommodityEquipmentRepository>();
            services.AddScoped<IEmailPlaceholderRepository, EmailPlaceholderRepository>();
            services.AddScoped<ICompanyChargeScheduleRepository, CompanyChargeScheduleRepository>();
            services.AddScoped<ICompanyChgSchInspectionGroupRepository, CompanyChgSchInspectionGroupRepository>();
            services.AddScoped<ICompanyChgSchInspGrpInspectionTypeRepository, CompanyChgSchInspGrpInspectionTypeRepository>();
            services.AddScoped<ICompanyInspectionTypeChargeRateRepository, CompanyInspectionTypeChargeRateRepository>();
            services.AddScoped<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>();
            services.AddScoped<ITaxonomyBusinessUnitRepository, TaxonomyBusinessUnitRepository>();
          
        }

        public void RegisterMasterService(IServiceCollection services)
        {
            services.AddScoped<IMasterRepository, MasterRepository>();
        }

        public void RegisterValidationService(IServiceCollection services)
        {
            services.AddScoped<ICountryValidationService, CountryValidationService>();
            services.AddScoped<ICountyValidationService, CountyValidationService>();
        }

    }
}
