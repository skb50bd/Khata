using Khata.Data.Core;
using Khata.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Khata.Data.Persistence
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

            services.AddScoped<ITrackingRepository<Product>, TrackingRepository<Product>>();
            services.AddScoped<ITrackingRepository<Service>, TrackingRepository<Service>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
