using Evolution.Email.Core.Services;
using Evolution.Email.Domain.Interfaces.Email;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Email.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        { 
            services.AddScoped<IEmailQueueService, EmailQueueService>();
           
        }
         
    }
}
