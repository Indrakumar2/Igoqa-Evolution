using Evolution.SupplierPO.Core.Services;
using Evolution.SupplierPO.Domain.Interfaces.SupplierPO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.SupplierPO.Core.BootStrappers
{
    public class Bootstrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ISupplierPOService, SupplierPOService>();
            services.AddScoped<ISupplierPOSubSupplierService, SupplierPOSubSupplierService>();
            services.AddScoped<ISupplierPONoteService, SupplierPONoteService>();
           services.AddScoped<ISupplierPODetailService, SupplierPODetailService>();
        }
    }
}
