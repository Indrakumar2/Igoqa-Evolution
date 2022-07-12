using Evolution.Contract.Domain.Interfaces.Data;
using Evolution.Contract.Domain.Interfaces.Validations;
using Evolution.Contract.Infrastructure.Data;
using Evolution.Contract.Infrastructure.Validations;
using Evolution.Contracts.Infrastructe.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Contract.Infrastructe.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IContractNoteRepository, ContractNoteRepository>();
            services.AddScoped<IContractScheduleRateRepository, ContractScheduleRateRepository>();
            services.AddScoped<IContractScheduleRepository, ContractScheduleRepository>();
            //services.AddScoped<IContractDocumentRepository, ContractDocumentRepository>();
            services.AddScoped<IContractMessageRepository, ContractMessageRepository>();
            services.AddScoped<IContractExchangeRateRepository, ContractExchangeRateRepository>();
            services.AddScoped<IContractInvoiceAttachmentRepository, ContractInvoiceAttachmentRepository>();
            services.AddScoped<IContractInvoiceReferenceTypeRepository, ContractInvoiceReferenceRepository>();



            //services.AddScoped<IContractDocumentValidationService, ContractDocumentValidationService>();
            services.AddScoped<IContractInvoiceReferenceTypeValidationService, ContractInvoiceReferencesValidationService>();
            services.AddScoped<IContractInvoiceAttachmentValidationService, ContractInvoiceAttachmentValidationService>();
            services.AddScoped<IContractExchangeRateValidationService, ContractExchangeRateValidationService>();
            services.AddScoped<IContractValidationService, ContractValidationService>();
            services.AddScoped<IContractNoteValidationService, ContractNoteValidationService>();
            services.AddScoped<IContractScheduleRateValidationService, ContractScheduleRateValidationService>();
            services.AddScoped<IContractScheduleValidationService, ContractScheduleValidationService>();
            services.AddScoped<IContractBatchRepository, ContractBatchRepository>();

        }
    }
}
