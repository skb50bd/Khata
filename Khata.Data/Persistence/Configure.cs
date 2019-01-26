using Khata.Data.Core;
using Khata.Domain;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Khata.Data.Persistence
{
    public static class Configure
    {
        public static IServiceCollection ConfigureData(
            this IServiceCollection services,
            string cnnString)
        {
            services.AddDbContext<KhataContext>(options =>
                options.UseSqlServer(cnnString)
                .EnableSensitiveDataLogging());
            //options.UseInMemoryDatabase("Khata"));

            services.AddDefaultIdentity<ApplicationUser>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddEntityFrameworkStores<KhataContext>();

            services.AddTransient<ITrackingRepository<Product>, TrackingRepository<Product>>();
            services.AddTransient<ITrackingRepository<Service>, TrackingRepository<Service>>();
            services.AddTransient<ITrackingRepository<Customer>, TrackingRepository<Customer>>();
            services.AddTransient<ITrackingRepository<DebtPayment>, DebtPaymentRepository>();
            services.AddTransient<ITrackingRepository<Sale>, SaleRepository>();
            services.AddTransient<ITrackingRepository<Invoice>, InvoiceRepository>();
            services.AddTransient<ITrackingRepository<Expense>, TrackingRepository<Expense>>();
            services.AddTransient<ITrackingRepository<Supplier>, TrackingRepository<Supplier>>();
            services.AddTransient<ITrackingRepository<SupplierPayment>, SupplierPaymentRepository>();
            services.AddTransient<ITrackingRepository<Purchase>, PurchaseRepository>();
            services.AddTransient<ITrackingRepository<Employee>, TrackingRepository<Employee>>();
            services.AddTransient<ITrackingRepository<SalaryIssue>, SalaryIssueRepository>();
            services.AddTransient<ITrackingRepository<SalaryPayment>, SalaryPaymentRepository>();
            services.AddTransient<ICashRegisterRepository, CashRegisterRepository>();
            services.AddTransient<IRepository<Withdrawal>, Repository<Withdrawal>>();
            services.AddTransient<IRepository<Deposit>, Repository<Deposit>>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
