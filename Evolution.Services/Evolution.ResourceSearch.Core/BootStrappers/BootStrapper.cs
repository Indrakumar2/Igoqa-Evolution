using Evolution.ResourceSearch.Core.Services;
using Evolution.ResourceSearch.Domain.Interfaces.ResourceSearch;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.ResourceSearch.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IResourceSearchService, ResourceSearchService>();
            services.AddScoped<IResourceSearchNoteService, ResourceSearchNoteService>();
            services.AddScoped<IOverrideResourceService, OverrideResourceService>();
            services.AddScoped<IResourceTechSpecSearchService, ResourceTechSpecSearchService>();
        }
    }
}
