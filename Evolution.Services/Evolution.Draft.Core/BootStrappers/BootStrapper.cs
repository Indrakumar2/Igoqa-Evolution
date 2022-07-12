using Evolution.Draft.Core.Services;
using Evolution.Draft.Domain.Interfaces.Draft;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Draft.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        { 
            services.AddScoped<IDraftService, DraftService>();
           
        }
         
    }
}
