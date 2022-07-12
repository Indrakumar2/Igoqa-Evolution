using Evolution.ILearn.Domain.Interfaces;
using Evolution.ILearn.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.ILearn.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterILearnRepository(services);
        }

        public void RegisterILearnRepository(IServiceCollection services)
        {
            services.AddScoped<ILearnRespositoryInterface, ILearnRepository>();
        }
    }
}
