using Evolution.Customer.Domain.Interfaces.Data;
using Evolution.Customer.Domain.Interfaces.Validations;
using Evolution.Customer.Infrastructure.Data;
using Evolution.Customer.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Customer.Infrastructe.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterCustomerRepository(services);
            this.RegisterCustomerValidationService(services);
        }

        public void RegisterCustomerRepository(IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
            services.AddScoped<ICustomerAssignmentReferenceRepository, CustomerAssignmentReferenceRepository>();
            services.AddScoped<ICustomerAccountReferenceRepository, CustomerAccountReferenceRepository>();
            services.AddScoped<ICustomerContactRepository, CustomerContactRepository>();
            services.AddScoped<ICustomerNoteRepository, CustomerNoteRepository>();
        }

        public void RegisterCustomerValidationService(IServiceCollection services)
        {
            services.AddScoped<ICustomerValidationService, CustomerValidationService>();
            services.AddScoped<ICustomerAddressValidationService, CustomerAddressValidationService>();
            services.AddScoped<ICustomerAssignmentReferenceValidationService, CustomerAssignmentReferenceValidationService>();
            services.AddScoped<ICustomerAccountReferenceValidationService, CustomerAccountReferenceValidationService>();
            services.AddScoped<ICustomerContactValidationService, CustomerContactValidationService>();
            services.AddScoped<ICustomerNoteValidationService, CustomerNoteValidationService>();
        }
    }
}