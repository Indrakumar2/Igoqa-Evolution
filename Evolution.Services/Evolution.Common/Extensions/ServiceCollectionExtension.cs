using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Evolution.Common.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static T ResolveService<T>(this IServiceCollection source) where T : class
        {
            var provider = source.BuildServiceProvider();
            if (provider != null)
                return provider.GetServices<T>().FirstOrDefault();
            else
                return null;
        }
    }
}
