using Evolution.Admin.Domain.Interfaces.Data;
using Evolution.Admin.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Admin.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            //Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IBatchProcess, BatchRepository>();
        }
    }
}
