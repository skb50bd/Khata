using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerService.CreateLogger();
            try
            {
                Log.Information("Starting Web Host");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration(
                       (hostingContext, config) =>
                       {
                           var env = hostingContext.HostingEnvironment;
                           config.AddJsonFile("appsettings.json",
                                      true,
                                      true)
                                 .AddJsonFile(
                                      $"appsettings.{env.EnvironmentName}.json",
                                      true, true);
                           config.AddEnvironmentVariables();
                       })
                  .UseSerilog()
                  .UseStartup<Startup>();
        }
    }
}
