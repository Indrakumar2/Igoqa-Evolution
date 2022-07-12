using Evolution.SupplierPO.Domain.Interfaces.Data;
using Evolution.SupplierPO.Domain.Interfaces.Validations;
using Evolution.SupplierPO.Domain.Models.Valildation;
using Evolution.SupplierPO.Infrastructure.Data;
using Evolution.SupplierPO.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.SupplierPO.Infrastructure.BootStrappers
{
    public class Bootstrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ISupplierPORepository, SupplierPORepository>();
            services.AddScoped<ISupplierPOSubSupplierRepository, SupplierPOSubSupplierRepository>();
            services.AddScoped<ISupplierPONoteRepository, SupplierPONoteRepository>();

            // Validation Services
            services.AddScoped<ISupplierPoValidationService, SupplierPOValidationService>();
            services.AddScoped<ISupplierPONoteValidationService, SuppilerPoNoteValidationService>();
            services.AddScoped<ISupplierPoSubSupplierValidationService, SupplierPoSubSupplierValidationService>();
        }
    }
}
