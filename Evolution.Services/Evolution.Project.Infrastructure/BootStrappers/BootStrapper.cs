using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Validations;
using Evolution.Project.Infrastructure.Data;
using Evolution.Project.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Project.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterProjectRepository(services);
            this.RegisterProjectValidationService(services);
        }
         
        public void RegisterProjectRepository(IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectValidationService, ProjectValidationService>();
            services.AddScoped<IProjectNoteRepository, ProjectNoteRepository>();
            services.AddScoped<IProjectInvoiceAttachmentRepository, ProjectInvoiceAttachmentRepository>();
            services.AddScoped<IProjectInvoiceReferenceRepository, ProjectInvoiceReferenceRepository>();
            services.AddScoped<IProjectClientNotificationRepository, ProjectClientNotificationRepository>();
            services.AddScoped<IProjectMessageRepository, ProjectMessageRepository>();

        }

        public void RegisterProjectValidationService(IServiceCollection services)
        {
            services.AddScoped<IInvoiceAttachmentValidationService, InvoiceAttachmentValidationService>();
            services.AddScoped<IInvoiceReferenceValidationService, InvoiceReferenceValidationService>();
            services.AddScoped<IProjectClientNotificationValidationService, ProjectClientNotificationValidationService>();
            services.AddScoped<IProjectNoteValidationService, ProjectNoteValidationService>();
        }

        //public void RegisterProjectMongoRepository(IServiceCollection services)
        //{
        //    services.AddScoped<IProjectMongoDocumentRepository, ProjectMongoDocumentRepository>();
        //}
    }
}
