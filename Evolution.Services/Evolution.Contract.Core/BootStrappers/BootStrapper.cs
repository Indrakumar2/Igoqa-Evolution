using Evolution.AuditLog.Core;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Evolution.Contract.Core.Services;
using Evolution.Contract.Domain.Interfaces.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Contract.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterContractService(services);
            //this.RegisterContractMongoService(services);
        }

        public void RegisterContractService(IServiceCollection services)
        {
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IContractNoteService, ContractNoteService>();
            services.AddScoped<IContractScheduleService, ContractScheduleService>();
            services.AddScoped<IContractScheduleRateService, ContractScheduleRateService>();
            services.AddScoped<IContractInvoiceReferenceTypeService, ContractInvoiceReferenceTypeService>();
            services.AddScoped<IContractInvoiceAttachmentService, ContractInvoiceAttachmentService>();
            services.AddScoped<IContractExchangeRateService, ContractExchangeRateService>(); 
            services.AddScoped<IContractDetailService, ContractDetailService>();  
            services.AddScoped<IContractDocumentService, ContractDocumentService>();
            services.AddScoped<IAuditLogger, AuditLogger>();
        }

        //public void RegisterContractMongoService(IServiceCollection services)
        //{
        //    services.AddScoped<IMongoDocumentService, ContractMongoDocumentService>();
        //}
    }
}
