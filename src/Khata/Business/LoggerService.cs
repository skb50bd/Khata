using System;
using System.IO;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Events;

namespace Business
{
    public static class LoggerService
    {
        private static readonly IConfiguration Configuration
            = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    false,
                    true)
                .AddEnvironmentVariables()
                .Build();
        public static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft",
                     LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                     Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/.log"),
                     fileSizeLimitBytes: 5_000_000,
                     rollingInterval: RollingInterval.Day,
                     rollOnFileSizeLimit: true,
                     shared: true,
                     flushToDiskInterval: TimeSpan.FromSeconds(1),
                     outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                 ).CreateLogger();
        }
    }
}
