using Evolution.Document.Domain.Interfaces.Data;
//using Evolution.Document.Domain.Interfaces.Validations;
using Evolution.Document.Domain.Validations;
//using Evolution.Document.Infrastructe.Validations;
using Evolution.Document.Infrastructure.Data;
using Evolution.Document.Infrastructure.Validations;
using Evolution.ValidationService.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Document.Infrastructe.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
          //  this.RegisterDocumentApprovalService(services);
            this.RegisterDocumentService(services);
            this.RegisterMongoDocumentService(services);
            this.RegisterMongoDocumentSyncService(services);
        }

        //public void RegisterDocumentApprovalService(IServiceCollection services)
        //{
        //    services.AddScoped<IDocumentApprovalRepository, DocumentApprovalRepository>();
        //    services.AddScoped<IDocumentApprovalValidationService, DocumentApprovalValidationService>();
        //}
        
        public void RegisterDocumentService(IServiceCollection services)
        {
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IValidationService, ValidationService.Services.ValidationService>();
            services.AddScoped<IDocumentValidationService, DocumentValidationService>();
        }

        public void RegisterMongoDocumentService(IServiceCollection services)
        {
            services.AddScoped<IMongoDocumentRepository, MongoDocumentRepository>();
        }

        public void RegisterMongoDocumentSyncService(IServiceCollection services)
        {
            services.AddScoped<IDocumentMongoSyncRepository, DocumentMongoSyncRepository>();
        }
    }
}
