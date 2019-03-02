using System.IO;

using ImportData.Models;

using Khata.Services.Mapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImportData
{
    using System.Threading.Tasks;

    using Khata.Data.Core;
    using Khata.Data.Persistence;
    using Khata.Domain;

    using Microsoft.EntityFrameworkCore;

    using Serilog;
    using Serilog.Events;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            await serviceProvider.GetService<App>().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(Log.Logger)
            );

            // build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app-settings.json", false)
                .Build();

            serviceCollection.ConfigureSqlData(configuration.GetValue<string>("DefaultConnection"));
            serviceCollection.ConfigureMapper();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            ConfigureConsole(configuration);
            // add app
            serviceCollection.AddTransient<App>();

        }

        private static void ConfigureConsole(IConfigurationRoot configuration)
        {
            System.Console.Title = configuration.GetSection("Configuration:ConsoleTitle").Value;
        }
    }

    public static class Configure
    {
        public static IServiceCollection ConfigureSqlData(this IServiceCollection services, string cnnString)
        {
            services.AddDbContext<KhataContext>(options =>
            //options.UseSqlite(cnnString));
            options.UseSqlServer(cnnString));
            //.EnableSensitiveDataLogging());
            //options.UseInMemoryDatabase("Khata"));

            services.AddTransient<ITrackingRepository<Outlet>, TrackingRepository<Outlet>>();
            services.AddTransient<ITrackingRepository<Product>, ProductRepository>();
            services.AddTransient<ITrackingRepository<Service>, TrackingRepository<Service>>();
            services.AddTransient<ITrackingRepository<Customer>, TrackingRepository<Customer>>();
            services.AddTransient<ITrackingRepository<DebtPayment>, DebtPaymentRepository>();
            services.AddTransient<ISaleRepository, SaleRepository>();
            services.AddTransient<ITrackingRepository<CustomerInvoice>, InvoiceRepository>();
            services.AddTransient<ITrackingRepository<Vouchar>, VoucharRepository>();
            services.AddTransient<ITrackingRepository<Expense>, TrackingRepository<Expense>>();
            services.AddTransient<ITrackingRepository<Supplier>, TrackingRepository<Supplier>>();
            services.AddTransient<ITrackingRepository<SupplierPayment>, SupplierPaymentRepository>();
            services.AddTransient<ITrackingRepository<Purchase>, PurchaseRepository>();
            services.AddTransient<ITrackingRepository<Employee>, TrackingRepository<Employee>>();
            services.AddTransient<ITrackingRepository<SalaryIssue>, SalaryIssueRepository>();
            services.AddTransient<ITrackingRepository<SalaryPayment>, SalaryPaymentRepository>();
            services.AddTransient<ICashRegisterRepository, CashRegisterRepository>();
            services.AddTransient<IRepository<Withdrawal>, WithdrawalRepository>();
            services.AddTransient<IRepository<Deposit>, DepositRepository>();
            services.AddTransient<ITrackingRepository<Refund>, RefundRepository>();
            services.AddTransient<ITrackingRepository<PurchaseReturn>, PurchaseReturnRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }

}