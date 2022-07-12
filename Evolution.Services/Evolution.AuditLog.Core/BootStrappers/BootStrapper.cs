using Evolution.AuditLog.Core.Services;
using Evolution.AuditLog.Domain.Interfaces.Audit;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.AuditLog.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterAuditLogService(services);
        }

        public void RegisterAuditLogService(IServiceCollection services)
        {
            services.AddScoped<ISqlAuditModuleService, SqlAuditModuleService>();
            services.AddScoped<ISqlAuditLogEventInfoService, SqlAuditLogEventInfoService>();
            services.AddScoped<ISqlAuditLogDetailInfoService, SqlAuditLogDetailInfoService>();
            services.AddScoped<IAuditLogger, AuditLogger>();
             services.AddScoped<IAuditSearchService, AuditSearchService>();
        }
    }
}