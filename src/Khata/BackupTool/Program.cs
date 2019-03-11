using System.IO;
using Microsoft.Extensions.Configuration;
using BackupRestore.Models;
using BackupRestore.Services;
using static System.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackupRestore
{
    public class Program
    {
        public static void Main()
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            ConfigureConsole(configuration);

            var cnnString = configuration.GetValue<string>("DefaultConnection");
            serviceCollection.AddDbContext<KhataContext>(opts => opts.UseSqlServer(cnnString));

            // add services
            serviceCollection.AddTransient<IBackupRestoreService, BackupRestoreService>();

            // add app
            serviceCollection.AddTransient<App>();
        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
        {
            Title = configuration.GetSection("Configuration:ConsoleTitle").Value;
        }
    }
}
