using Evolution.Document.Core.Services;
using Evolution.Document.Domain.Interfaces.Documents;
using Evolution.FileExtractor.Interfaces;
using Evolution.FileExtractor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Document.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterDocumentService(services);
          //  this.RegisterDocumentApprovalService(services);

            this.RegisterDocumentMongoSyncService(services);
            this.RegisterMongoDocumentService(services);
        }

        public void RegisterDocumentService(IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
        }
        
        //public void RegisterDocumentApprovalService(IServiceCollection services)
        //{
        //    services.AddScoped<IDocumentApprovalService, DocumentApprovalService>();
        //}

        public void RegisterDocumentMongoSyncService(IServiceCollection services)
        {
            services.AddScoped<IDocumentMongoSyncService, DocumentMongoSyncService>(); 
                services.AddScoped<IMongoDocumentExtractor, MongoDocumentExtractor>();
        }
        
        public void RegisterMongoDocumentService(IServiceCollection services)
        {
            services.AddScoped<IMongoDocumentService, MongoDocumentService>();
        }
    }
}
