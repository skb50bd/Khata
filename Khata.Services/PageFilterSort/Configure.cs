using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Sieve.Services;

namespace Khata.Web.PagingSortingSearching
{
    public static class Configure
    {
        public static IServiceCollection ConfigureSieve(this IServiceCollection services, IConfiguration Config)
        {
            //services.Configure<SieveOptions>(Config.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, KhataSieveProcessor>();

            return services;
        }
    }
}