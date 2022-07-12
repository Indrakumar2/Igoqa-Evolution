using Evolution.Security.Core.Services;
using Evolution.Security.Domain.Interfaces.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Security.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterSecurityService(services);
        }

        public void RegisterSecurityService(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IRoleService, RoleService>(); 
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IModuleDetailService, ModuleDetailService>();
            services.AddScoped<IUserDetailService, UserDetailService>();
            services.AddScoped<IRoleDetailService, RoleDetailService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
        }
    }
}