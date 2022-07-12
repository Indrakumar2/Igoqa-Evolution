using Evolution.Finance.Core.Services;
using Evolution.Finance.Domain.Interfaces.Data;
using Evolution.Finance.Domain.Interfaces.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Project.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterFinanceServices(services);
        }
        public void RegisterFinanceServices(IServiceCollection services)
        {
            services.AddScoped<IFinanceService, FinanceService>();
        }
    }
}
