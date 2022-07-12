using Evolution.Supplier.Core.Services;
using Evolution.Supplier.Domain.Interfaces.Suppliers;
using Evolution.SupplierContacts.Domain.Interfaces.Suppliers;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Supplier.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierContactService, SupplierContactService>();
            services.AddScoped<ISupplierNoteService, SupplierNoteService>();
            services.AddScoped<ISupplierDetailService, SupplierDetailService>();
            services.AddScoped<ISupplierPerformanceReportService, SupplierPerformanceReportService>();

        }
    }
}
