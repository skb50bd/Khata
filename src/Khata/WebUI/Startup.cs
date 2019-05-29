using System.Globalization;

using Business;
using Business.Abstractions;
using Business.Email;
using Business.Mapper;
using Business.PageFilterSort;
using Business.Reports;

using Data.Persistence;

using Domain;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.Swagger;

using WebUI.Hubs;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CultureInfo.DefaultThreadCurrentCulture =
                Configuration.GetValue<CultureInfo>("OutletOptions:Culture");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var dbProvider = 
                Configuration
                   .GetValue<DbProvider>(
                        "Settings:DbProvider");

            string connectionString;
            switch (dbProvider)
            {
                case DbProvider.SQLServer:
                    connectionString =
                        Configuration
                           .GetConnectionString(
                                "SQLServerConnection");
                    break;
                case DbProvider.PostgreSQL:
                    connectionString =
                        Configuration
                           .GetConnectionString(
                                "PostgreSQLConnection");
                    break;
                case DbProvider.SQLite:
                    connectionString =
                        Configuration
                           .GetConnectionString(
                                "SQLiteConnection");
                    break;
                default:
                    connectionString =
                        Configuration
                           .GetConnectionString(
                                "SQLiteConnection");
                    break;

            }

            services.ConfigureData(dbProvider, connectionString);

            services.ConfigureSieve();

            services.AddReports();

            services.ConfigureMapper();

            services.ConfigureCrudServices();

            services.AddPolicies();

            services.AddWebOptimizer();
            services.AddMvc()
                .AddJsonOptions(options =>
                     {
                         options.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();

                         options.SerializerSettings.DefaultValueHandling =
                            DefaultValueHandling.Include;

                         options.SerializerSettings.ReferenceLoopHandling =
                             ReferenceLoopHandling.Serialize;

                         options.SerializerSettings.NullValueHandling =
                             NullValueHandling.Ignore;
                     })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddAuthorizationDefinition();

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "Khata_API",
                        Version = "v1"
                    });
            }
            );

            services.AddSignalR();

            services.Configure<OutletOptions>(
                Configuration.GetSection("OutletOptions"));

            services.Configure<KhataSettings>(
                Configuration.GetSection("Settings"));

            services
               .AddTransient<
                    IRazorViewToStringRenderer,
                    RazorViewToStringRenderer>();

            services.Configure<Settings>(
                Configuration.GetSection("EmailSettings"));
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IFileService fileService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseWebOptimizer();
                app.UseHttpsRedirection();
                app.UseCookiePolicy();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ReportsHub>("/Reports");
            });

            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new
                    {
                        controller = "Home",
                        action = "Index"
                    });
            });

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Khata_API");
            });

            Seeder.SeedUsers(roleManager, userManager)
                     .Wait();
            Seeder.SeedImages(fileService);
        }
    }
}
