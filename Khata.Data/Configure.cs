using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Khata.Data
{
    public static class Configure
    {
        public static IServiceCollection ConfureData(IServiceCollection services, string conncectionstring)
        {
            services.AddDbContext<KhataContext>(options => options.UseSqlServer(conncectionstring));


            return services;
        }
    }
}
