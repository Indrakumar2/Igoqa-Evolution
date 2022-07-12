using Evolution.Customer.Core.Services;
using Evolution.Customer.Domain.Interfaces.Customers;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Customer.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterCustomerService(services); 
        }

        public void RegisterCustomerService(IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<ICustomerAssignmentReferenceService, CustomerAssignmentReferenceService>();
            services.AddScoped<ICustomerAccountReferenceService, CustomerAccountReferenceService>();
            services.AddScoped<ICustomerContactService, CustomerContactService>();
            services.AddScoped<ICustomerNoteService, CustomerNoteService>();
            services.AddScoped<ICustomerDetailService, CustomerDetailService>();
        }
 
    }
}
