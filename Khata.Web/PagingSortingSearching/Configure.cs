using Microsoft.Extensions.DependencyInjection;

using Sieve.Services;

namespace Khata.Web.PagingSortingSearching
{
    public static class Configure
    {
        public static IServiceCollection ConfigureSieve(this IServiceCollection services)
        {
            services.AddScoped<ISieveProcessor, KhataSieveProcessor>();

            return services;
        }
        
    }
}