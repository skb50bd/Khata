using System.Globalization;

using Khata.Data.Persistence;
using Khata.Domain;
using Khata.Services.CRUD;
using Khata.Services.Mapper;
using Khata.Services.PageFilterSort;
using Khata.Services.Reports;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-BD");
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

            var connectionString = Configuration
                .GetConnectionString("DefaultConnection");

            services.ConfigureData(connectionString);

            services.ConfigureSieve(Configuration);

            services.AddReports();

            services.ConfigureMapper();

            services.ConfigureCrudServices();

            services.AddAuthorization(options =>
            {

                options.AddPolicy("AdminRights", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserRights", policy => policy.RequireRole("User"));
            });

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
                    }
                ); ;

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

            services.Configure<OutletOptions>(Configuration.GetSection("OutletOptions"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ReportsHub>("/Reports");
            });

            app.UseMvc();

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "Khata_API");
            });

            SeedUsers.Seed(roleManager, userManager).Wait();
        }
    }
}
