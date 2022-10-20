using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Brotal.Extensions;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;

using Serilog;

using static Business.LoggerService;

namespace WebUI;

public class Program {
    public static void Main (string[] args) {
        var confBuilder =
            new ConfigurationBuilder()
                .AddJsonFile(
                    "appsettings.json",
                    true,
                    true);
        var conf = confBuilder.Build();
        CreateLogger(conf);

        try
        {
            var isService = 
                !(Debugger.IsAttached 
                  || args.Contains ("--console") 
                  || !Platform.IsWindows
                  || !conf.GetValue<bool>("IsService"));

            if (isService) {
                var pathToExe = Process.GetCurrentProcess ().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName (pathToExe);
                Directory.SetCurrentDirectory (pathToContentRoot);
            }

            var builder = CreateWebHostBuilder (
                args.Where (arg => arg != "--console").ToArray ());

            var host = builder.Build ();

            if (isService) {
                Log.Information ("Starting Web Host As Service");
                host.RunAsService ();
            } else {
                Log.Information ("Starting Web Host");
                host.Run ();
            }
            // CreateWebHostBuilder (args).Build ().Run ();
        } catch (Exception e) {
            Log.Fatal (e, "Host terminated unexpectedly!");
        } finally {
            Log.CloseAndFlush ();
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder (string[] args) {
        return WebHost.CreateDefaultBuilder (args)
            .ConfigureAppConfiguration (
                (hostingContext, config) => {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile (
                            "appsettings.json",
                            true,
                            true)
                        .AddJsonFile (
                            $"appsettings.{env.EnvironmentName}.json",
                            true,
                            true);
                    config.AddEnvironmentVariables ();
                })
            .UseSerilog ()
            .UseStartup<Startup> ();
    }
}