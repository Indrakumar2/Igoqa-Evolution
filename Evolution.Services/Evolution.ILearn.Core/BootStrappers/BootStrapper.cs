
using Evolution.ILearn.Core.Services;
using Evolution.ILearn.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Home.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterILearnService(services);
        }

        public void RegisterILearnService(IServiceCollection services)
        {
            services.AddScoped<ILearnInterface, ILearnService>();
        }

    }
}
