using System.Globalization;

using Business;
using Business.Auth;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var dbProvider = Configuration.GetValue<DbProvider>("Settings:DbProvider");
            string connectionString;
            switch (dbProvider)
            {
                case DbProvider.SQLServer:
                    connectionString =
                        Configuration.GetConnectionString("SQLServerConnection");
                    break;
                case DbProvider.PostgreSQL:
                    connectionString =
                        Configuration.GetConnectionString("PostgreSQLConnection");
                    break;
                case DbProvider.SQLite:
                    connectionString =
                        Configuration.GetConnectionString("SQLiteConnection");
                    break;
                default:
                    connectionString =
                        Configuration.GetConnectionString("SQLiteConnection");
                    break;

            }

            services.ConfigureData(dbProvider, connectionString);

            services.ConfigureSieve();

            services.AddReports();

            services.ConfigureMapper();

            services.ConfigureCrudServices();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRights",
                    policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserRights",
                    policy => policy.RequireRole("User"));
            });

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
                .AddRazorPagesOptions(
                    options =>
                    {
                        options.Conventions.AuthorizeFolder("/Customers", "AdminRights");
                        options.Conventions.AuthorizeFolder("/DebtPayments", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Deposits", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Employees", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Cash", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Dashboard", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Expenses", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Invoices", "UserRights");
                        options.Conventions.AuthorizeFolder("/Products", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Purchases", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Refunds", "AdminRights");
                        options.Conventions.AuthorizeFolder("/SalaryIssues", "AdminRights");
                        options.Conventions.AuthorizeFolder("/SalaryPayments", "AdminRights");
                        options.Conventions.AuthorizePage("/Sales/Create", "UserRights");
                        options.Conventions.AuthorizePage("/Sales/Index", "UserRights");
                        options.Conventions.AuthorizePage("/Sales/Details", "UserRights");
                        options.Conventions.AuthorizeFolder("/Services", "AdminRights");
                        options.Conventions.AuthorizeFolder("/SupplierPayments", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Suppliers", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Vouchars", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Withdrawals", "AdminRights");
                        options.Conventions.AuthorizeFolder("/Reporting", "AdminRights");
                    }
                );

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

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<OutletOptions>(
                Configuration.GetSection("OutletOptions"));
            services.Configure<AuthMessageSenderOptions>(
                Configuration.GetSection("SendGrid"));
            services.Configure<KhataSettings>(
                Configuration.GetSection("Settings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //app.UseResponseCompression();
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
                    new { controller = "Home", action = "Index" });
            });

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Khata_API");
            });

            SeedUsers.Seed(roleManager, userManager).Wait();
        }
    }
}
