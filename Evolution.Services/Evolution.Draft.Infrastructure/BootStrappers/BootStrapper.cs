using Evolution.Draft.Domain.Interfaces.Data;
using Evolution.Draft.Domain.Interfaces.Validations;
using Evolution.TechnicalSpecialist.Infrastructure.Data;
using Evolution.TechnicalSpecialist.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Draft.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IDraftRepository, DraftRepository>();
            services.AddScoped<IDraftValidationService, DraftValidationService>();
             
        }

          
    }
}
