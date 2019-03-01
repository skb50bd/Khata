using Khata.Data.Core;
using Khata.Domain;
using Khata.Queries;

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
            //options.UseSqlite(cnnString));
            options.UseSqlServer(cnnString));
            //.EnableSensitiveDataLogging());
            //options.UseInMemoryDatabase("Khata"));
            services.AddDefaultIdentity<ApplicationUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            }).AddRoles<IdentityRole>()
              .AddDefaultUI(UIFramework.Bootstrap4)
              .AddEntityFrameworkStores<KhataContext>();

            services.AddTransient<ITrackingRepository<Outlet>, TrackingRepository<Outlet>>();
            services.AddTransient<ITrackingRepository<Product>, ProductRepository>();
            services.AddTransient<ITrackingRepository<Service>, ServiceRepository>();
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

            services.AddTransient<IIndividualReportRepository<CustomerReport>, IndividualReportRepository<CustomerReport>>();
            services.AddTransient<IIndividualReportRepository<SupplierReport>, IndividualReportRepository<SupplierReport>>();
            services.AddTransient<IReportRepository<AssetReport>, ReportRepository<AssetReport>>();
            services.AddTransient<IReportRepository<LiabilityReport>, ReportRepository<LiabilityReport>>();
            services.AddTransient<IReportRepository<DailyIncomeReport>, ReportRepository<DailyIncomeReport>>();
            services.AddTransient<IReportRepository<DailyExpenseReport>, ReportRepository<DailyExpenseReport>>();
            services.AddTransient<IReportRepository<DailyPayableReport>, ReportRepository<DailyPayableReport>>();
            services.AddTransient<IReportRepository<DailyReceivableReport>, ReportRepository<DailyReceivableReport>>();
            services.AddTransient<IReportRepository<WeeklyIncomeReport>, ReportRepository<WeeklyIncomeReport>>();
            services.AddTransient<IReportRepository<WeeklyExpenseReport>, ReportRepository<WeeklyExpenseReport>>();
            services.AddTransient<IReportRepository<WeeklyPayableReport>, ReportRepository<WeeklyPayableReport>>();
            services.AddTransient<IReportRepository<WeeklyReceivableReport>, ReportRepository<WeeklyReceivableReport>>();
            services.AddTransient<IReportRepository<MonthlyIncomeReport>, ReportRepository<MonthlyIncomeReport>>();
            services.AddTransient<IReportRepository<MonthlyExpenseReport>, ReportRepository<MonthlyExpenseReport>>();
            services.AddTransient<IReportRepository<MonthlyPayableReport>, ReportRepository<MonthlyPayableReport>>();
            services.AddTransient<IReportRepository<MonthlyReceivableReport>, ReportRepository<MonthlyReceivableReport>>();
            services.AddTransient<IReportRepository<PerDayReport>, ReportRepository<PerDayReport>>();


            return services;
        }
    }
}
