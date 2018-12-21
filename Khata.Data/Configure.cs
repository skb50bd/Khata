using Khata.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Khata.Data
{
    public static class Configure
    {
        public static IServiceCollection ConfigureData(
            this IServiceCollection services, 
            string conncectionString)
        {
            services.AddDbContext<KhataContext>(options =>
                options.UseSqlServer(conncectionString));

            services.AddDefaultIdentity<ApplicationUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<KhataContext>();

            return services;
        }
    }
}
