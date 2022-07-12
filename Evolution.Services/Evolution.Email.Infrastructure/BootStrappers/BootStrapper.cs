using Evolution.Email.Domain.Interfaces.Data;
using Evolution.Email.Domain.Interfaces.Validations;
using Evolution.Email.Infrastructure.Data;
using Evolution.Email.Infrastructure.Validations;
using Evolution.Master.Domain.Interfaces.Data;
using Evolution.Master.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Email.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IEmailValidationService, EmialValidationService>();
            services.AddScoped<IMasterRepository, MasterRepository>();

        }

          
    }
}
