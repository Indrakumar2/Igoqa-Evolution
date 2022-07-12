using Evolution.DbRepository.Interfaces.Master;
using Evolution.DbRepository.Services.Master;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.DbRepository.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterDataService(services);           
            services.AddScoped<ITaxonomySubCategoryRepository, TaxonomySubCategoryRepository>();
            services.AddScoped<ITaxonomyServiceRepository, TaxonomyServiceRepository>();
            services.AddScoped<ITechnicalSpecialistCustomerRepository, TechnicalSpecialistCustomerRepository>();
            services.AddScoped<ICustomerCommodityRepository, CustomerCommodityRepository>();
            services.AddScoped<ITimeOffRequestCategoryRepository, TimeOffRequestCategoryRepository>();
        }

        public void RegisterDataService(IServiceCollection services)
        {
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountyRepository, CountyRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
        }
    }
}
