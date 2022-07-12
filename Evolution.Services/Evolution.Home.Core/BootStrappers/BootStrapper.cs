using Evolution.Home.Core.Services;
using Evolution.Home.Domain.Interfaces.Homes;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Home.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterHomeService(services);
        }

        public void RegisterHomeService(IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IMyTaskService, MyTaskService>();
        }
    }
}
