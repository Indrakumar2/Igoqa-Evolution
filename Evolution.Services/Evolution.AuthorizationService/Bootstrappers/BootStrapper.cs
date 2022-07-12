using Evolution.AuthorizationService.Infrastructures.Data;
using Evolution.AuthorizationService.Interfaces;
using Evolution.AuthorizationService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.AuthorizationService.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtHandler, JwtHandler>();
            //services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUserReposiotry, UserRepository>();
            services.AddScoped<IRefreshTokenReposiotry, RefreshTokenRepository>();
            services.AddScoped<IAuthClientRepository, AuthClientRepository>();
        }
    }
}
