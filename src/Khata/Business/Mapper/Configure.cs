using AutoMapper;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper
{
    public static class Configure
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            _ = services.AddAutoMapper();
            //AutoMapper.Mapper.Configuration.AssertConfigurationIsValid();
            return services;
        }
    }
}