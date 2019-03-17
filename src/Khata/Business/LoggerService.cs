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
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(
                    @"log\log.txt",
                    fileSizeLimitBytes: 5_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1)
                ).CreateLogger();
        }
    }
}
