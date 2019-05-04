using AutoMapper;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper
{
    public static class Configure
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Entity).Assembly);
            //AutoMapper.Mapper.Configuration.AssertConfigurationIsValid();
            return services;
        }
    }
}