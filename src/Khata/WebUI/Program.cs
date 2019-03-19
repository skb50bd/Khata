using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

using Serilog;

using static Business.LoggerService;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateLogger();
            try
            {
                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                if (isService)
                {
                    var pathToExe         = Process.GetCurrentProcess().MainModule.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                    Directory.SetCurrentDirectory(pathToContentRoot);
                }

                var builder = CreateWebHostBuilder(
                    args.Where(arg => arg != "--console").ToArray());

                var host = builder.Build();

                if (isService)
                {
                    // To run the app without the CustomWebHostService change the
                    // next line to host.RunAsService();
                    //host.RunAsCustomService();
                    host.RunAsService();
                }
                else
                {
                    host.Run();
                }
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
                           config.AddJsonFile(
                                  "appsettings.json",
                                  true,
                                  true)
                             .AddJsonFile(
                                  $"appsettings.{env.EnvironmentName}.json",
                                  true,
                                  true);
                           config.AddEnvironmentVariables();
                       })
                  .UseSerilog()
                  .UseStartup<Startup>();
        }
    }
}
