using Evolution.Security.Domain.Interfaces.Data;
using Evolution.Security.Domain.Interfaces.Validations;
using Evolution.Security.Infrastructure.Data;
using Evolution.Security.Infrastructure.Validations;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Security.Infrastructure.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            this.RegisterService(services);
            this.RegisterValidationService(services);
        }

        private void RegisterService(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>(); 
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IUserDetailRepository, UserDetailRepository>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleDetailRepository, RoleDetailRepository>();

            services.AddScoped<IActivityRepository, ActivityRepository>();

            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IModuleDetailRepository, ModuleDetailRepository>();
            
            services.AddScoped<IApplicationMenuRepository, ApplicationMenuRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ICustomerUserProjectRepository, CustomerUserProjectRepository>();

            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        }

        private void RegisterValidationService(IServiceCollection services)
        {
            services.AddScoped<IActivityValidationService, ActivityValidationService>();
            services.AddScoped<IModuleValidationService, ModuleValidationService>();
            services.AddScoped<IRoleValidationService, RoleValidationService>();
            services.AddScoped<IUserValidationService, UserValidationService>();
            services.AddScoped<IUserRoleValidationService, UserRoleValidationService>();
            services.AddScoped<IUserTypeValidationService, UserTypeValidationService>();
            //services.AddScoped<IModuleActivityValidationService, ModuleActivityValidationService>();
            //services.AddScoped<IRoleActivityValidationService, RoleActivityValidationService>();
        }
    }
}
