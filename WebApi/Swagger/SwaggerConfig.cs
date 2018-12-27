using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace WebApi.Swagger
{
    public static class SwaggerConfig
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Khata API",
                    Description = "Khata API provides all the necessary backend services for the Khata Application",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Shakib Haris", Email = "skb50bd@gmail.com", Url = "https://github.com/skb50bd" }
                });
            });

            return services;
        }

        public static IApplicationBuilder EngageSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Khata API");
            });

            return app;
        }
    }
}