using System.Globalization;

using Business;
using Business.Email;
using Business.Mapper;
using Business.PageFilterSort;
using Business.Reports;

using Data.Persistence;

using Domain;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebUI.Hubs;

namespace WebUI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        CultureInfo.DefaultThreadCurrentCulture =
            Configuration.GetValue<CultureInfo>("OutletOptions:Culture");
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        var dbProvider = 
            Configuration
                .GetValue<DbProvider>(
                    "Settings:DbProvider");

        var connectionString = dbProvider switch
        {
            DbProvider.SQLServer  => Configuration.GetConnectionString("SQLServerConnection"),
            DbProvider.PostgreSQL => Configuration.GetConnectionString("PostgreSQLConnection"),
            DbProvider.SQLite     => Configuration.GetConnectionString("SQLiteConnection"),
            _                     => Configuration.GetConnectionString("SQLiteConnection")
        };

        services.ConfigureData(dbProvider, connectionString);

        services.ConfigureSieve();

        services.AddReports();

        services.ConfigureMapper();

        services.ConfigureCrudServices();

        services.AddPolicies();

        services.AddMvc()
            .AddNewtonsoftJson(options =>
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
            .AddAuthorizationDefinition();

        services.AddHttpContextAccessor();

        services.AddSwaggerGen();

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
        IHostEnvironment env,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseSwagger();
            // app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
        }
        
        app.UseStaticFiles();
        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(
            builder =>
            {
                builder.MapControllers();
                builder.MapRazorPages();
                builder.MapHub<ReportsHub>("/Reports");
            });

        SeedUsers
            .Seed(roleManager, userManager)
            .Wait();
    }
}