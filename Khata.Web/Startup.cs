using System.Globalization;

using Khata.Data.Persistence;
using Khata.Services.Mapper;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json.Serialization;

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

            services.ConfigureMapper();

            services.AddMvc()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver =
                        new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddResponseCompression(options =>
            //{
            //    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            //    {
            //        MediaTypeNames.Application.Octet,
            //        WasmMediaTypeNames.Application.Wasm,
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
            //app.UseBlazor<Khata.Client.Startup>();
        }
    }
}
