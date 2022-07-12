using Evolution.Google.Core.Repositories;
using Evolution.Google.Core.Services;
using Evolution.Google.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Evolution.Google.Core.BootStrappers
{
    public class BootStrapper
    {
        public void Register(IServiceCollection services)
        {
            RegisterGoogleService(services);

            RegisterMongoService(services);
        }

        public void RegisterGoogleService(IServiceCollection services)
        {
            services.AddScoped<IGeoCoordinateService, GeoCoordinateService>();
            services.AddScoped<IDistanceService, DistanceService>();
        }

        public void RegisterMongoService(IServiceCollection services)
        {
            services.AddScoped<IMongoGeoCoordinateService, MongoGeoCoordinateService>();
            services.AddScoped<IMongoGeoCoordinateRepository, MongoGeoCoordinateReposiotry>();
        }
    }
}
