using Microsoft.Extensions.DependencyInjection;

namespace Business.PageFilterSort;

public static class Configure
{
    public static IServiceCollection ConfigureSieve(
        this IServiceCollection services)
    {
        services.AddScoped<PfService>();
        return services;
    }
}