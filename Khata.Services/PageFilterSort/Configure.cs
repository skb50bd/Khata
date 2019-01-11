using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.PageFilterSort
{
    public static class Configure
    {
        public static IServiceCollection ConfigureSieve(this IServiceCollection services, IConfiguration Config)
        {
            //services.Configure<SieveOptions>(Config.GetSection("PfService"));
            //services.AddScoped<ISieveProcessor, KhataSieveProcessor>();
            services.AddScoped<PfService>();


            return services;
        }
    }
}