﻿using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.Mapper
{
    public static class Configure
    {
        public static IServiceCollection ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(m =>
            {
                m.AddProfile<MappingProfile>();
            });
            //AutoMapper.Mapper.Configuration.AssertConfigurationIsValid();
            return services;
        }
    }
}