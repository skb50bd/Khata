using Khata.Console.Models;
using Khata.Console.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using static System.Console;

namespace Khata.Console
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection)
        {
            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", optional: false, reloadOnChange: true)
                .Build();

            //add Configuration
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            ConfigureConsole(configuration);

            // add and Configure Serilog
            serviceCollection.AddSerilogServices(configuration);



            // add services
            serviceCollection.AddTransient<ITestService, TestService>();

            // add app
            serviceCollection.AddTransient<App>();

            return serviceCollection;
        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
            => Title = configuration.GetSection("Configuration:ConsoleTitle").Value;

        public static IServiceCollection AddSerilogServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

    }
}
