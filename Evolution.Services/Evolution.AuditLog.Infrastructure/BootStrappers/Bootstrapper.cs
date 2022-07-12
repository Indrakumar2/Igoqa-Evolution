using AutoMapper;
using Evolution.AuditLog.Domain.Interfaces.Data;
using Evolution.AuditLog.Domain.Interfaces.Validations;
using Evolution.AuditLog.Infrastructure.Data;
using Evolution.AuditLog.Infrastructure.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.AuditLog.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services, DbContext dbContext, IMapper mapper)
        {
            this.RegisterService(services,dbContext,mapper);
            this.RegisterValidationService(services);
        }

        public void Register(IServiceCollection services)
        {
            this.RegisterService(services,null,null);
            this.RegisterValidationService(services);
        }

        private void RegisterService(IServiceCollection services, DbContext dbContext,IMapper mapper)
        {
            if (dbContext != null && mapper != null)
            {
                services.AddScoped<ISqlAuditLogDetailRepository,SqlAuditLogDetailRepository>();
                services.AddScoped<ISqlAuditLogEventReposiotry,SqlAuditLogEventReposiotry>();
                services.AddScoped<ISqlAuditModuleRepository, SqlAuditModuleRepository>();
                 services.AddScoped<IAuditSearchRepository, AuditSearchReposiotry>();
            }
            else
            {
                services.AddScoped<ISqlAuditLogDetailRepository, SqlAuditLogDetailRepository>();
                services.AddScoped<ISqlAuditLogEventReposiotry, SqlAuditLogEventReposiotry>();
                services.AddScoped<ISqlAuditModuleRepository, SqlAuditModuleRepository>();
                 services.AddScoped<IAuditSearchRepository, AuditSearchReposiotry>();
            }
        }

        private void RegisterValidationService(IServiceCollection services)
        {
            services.AddScoped<ISqlAuditLogEventValidationService, SqlAuditLogEventValidationService>();
            services.AddScoped<ISqlAditLogDetailValidationService, SqlAditLogDetailValidationService>();
        }
    }
}
