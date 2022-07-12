using Evolution.Contract.Infrastructure.Data;
using Evolution.Supplier.Domain.Interfaces.Data;
using Evolution.Supplier.Domain.Interfaces.Validations;
using Evolution.Supplier.Infrastructure.Data;
using Evolution.Supplier.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Supplier.Infrastructe.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierContactRepository, SupplierContactRepository>();
            services.AddScoped<ISupplierPerfomanceRepository, SupplierPerformanceRepository>();

            services.AddScoped<ISupplierNoteRepository, SupplierNoteRepository>();
            services.AddScoped<ISupplierNoteValidationService, SupplierNoteValidationService>();
            services.AddScoped<ISupplierContactValidationService, SupplierContactValidationService>();
            services.AddScoped<ISupplierValidationService, SupplierValidationService>();
        }
    }
}
