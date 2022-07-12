using Evolution.Project.Core.Services;
using Evolution.Project.Domain.Interfaces.Data;
using Evolution.Project.Domain.Interfaces.Projects;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Project.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterProjectServices(services);
            //this.RegisterProjectMongoService(services);
        }
        public void RegisterProjectServices(IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>(); 
            services.AddScoped<IProjectNotesService, ProjectNotesService>();
            services.AddScoped<IProjectInvoiceAttachmentService, ProjectInvoiceAttachmentService>();
            services.AddScoped<IProjectInvoiceReferenceService, ProjectInvoiceReferenceService>();
            services.AddScoped<IProjectClientNotificationService, ProjectClientNotificationService>();
            services.AddScoped<IProjectDetailService, ProjectDetailService>();
            services.AddScoped<IProjectDocumentService, ProjectDocumentService>();
        }

        //public void RegisterProjectMongoService(IServiceCollection services)
        //{
        //    services.AddScoped<IMongoDocumentService, ProjectMongoDocumentService>();
        //}
    }
}
