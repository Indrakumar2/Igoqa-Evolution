using Evolution.ResourceSearch.Domain.Interfaces.Data;
using Evolution.ResourceSearch.Domain.Interfaces.Validations;
using Evolution.ResourceSearch.Infrastructure.Data;
using Evolution.ResourceSearch.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.ResourceSearch.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IResourceSearchRepository, ResourceSearchRepository>();
            services.AddScoped<IResourceSearchNoteRepository, ResourceSearchNoteRepository>();
            services.AddScoped<IOverrideResourceRepository, OverrideResourceRepository>();

            services.AddScoped<IResourceSearchValidation, ResourceSearchValidationService>();
            services.AddScoped<IOverrideResourceValidation, OverrideResourceValidation>();
        }
    }
}
