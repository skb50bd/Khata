using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper;

public static class Configure
{
    public static IServiceCollection ConfigureMapper(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        return services;
    }
}