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
                options.UseSqlServer(cnnString));
            //options.UseInMemoryDatabase("Khata"));

            services.AddDefaultIdentity<ApplicationUser>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddEntityFrameworkStores<KhataContext>();

            services.AddScoped<ITrackingRepository<Product>, TrackingRepository<Product>>();
            services.AddScoped<ITrackingRepository<Service>, TrackingRepository<Service>>();
            services.AddScoped<ITrackingRepository<Customer>, TrackingRepository<Customer>>();
            services.AddScoped<ITrackingRepository<DebtPayment>, DebtPaymentRepository>();
            services.AddScoped<ITrackingRepository<Sale>, SaleRepository>();
            services.AddScoped<ITrackingRepository<Expense>, TrackingRepository<Expense>>();
            services.AddScoped<ITrackingRepository<Supplier>, TrackingRepository<Supplier>>();
            services.AddScoped<ITrackingRepository<SupplierPayment>, SupplierPaymentRepository>();
            services.AddScoped<ITrackingRepository<Purchase>, PurchaseRepository>();
            services.AddScoped<ITrackingRepository<Employee>, TrackingRepository<Employee>>();
            services.AddScoped<ITrackingRepository<SalaryIssue>, SalaryIssueRepository>();
            services.AddScoped<ITrackingRepository<SalaryPayment>, SalaryPaymentRepository>();
            services.AddScoped<ICashRegisterRepository, CashRegisterRepository>();
            services.AddScoped<IRepository<Withdrawal>, Repository<Withdrawal>>();
            services.AddScoped<IRepository<Deposit>, Repository<Deposit>>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
