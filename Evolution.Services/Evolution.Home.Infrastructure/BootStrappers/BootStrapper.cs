using Evolution.Home.Domain.Interfaces.Data;
using Evolution.Home.Domain.Models.Validations;
using Evolution.Home.Infrastructure.Data;
using Evolution.Home.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Home.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        { 
            services.AddScoped<IMyTaskRepository, MyTaskRepository>();

            services.AddScoped<IMyTaskValidationService, MyTaskValidationService>();
        }
    }
}
