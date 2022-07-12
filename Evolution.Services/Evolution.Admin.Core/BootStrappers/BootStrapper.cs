using Evolution.Admin.Core.Services;
using Evolution.Admin.Domain.Interfaces.Admins;
using Evolution.Contract.Core.Services;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Admin.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<IContractScheduleService, ContractScheduleService>();
        }
    }
}
