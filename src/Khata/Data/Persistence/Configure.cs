using Data.Core;
using Data.Persistence.Reports;
using Data.Persistence.Repositories;

using Domain;
using Domain.Reports;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Persistence;

public static class Configure
{
    public static IServiceCollection ConfigureData(
        this IServiceCollection services,
        DbProvider dbProvider,
        string cnnString)
    {
        switch (dbProvider)
        {
            case DbProvider.SQLite:
                services.AddDbContext<KhataContext>(options =>
                    options.UseSqlite(cnnString)
                );
                break;

            case DbProvider.SQLServer:
                services.AddDbContext<KhataContext>(options =>
                    options.UseSqlServer(cnnString)
                );
                break;

            case DbProvider.PostgreSQL:
                services.AddDbContext<KhataContext>(options => 
                    options.UseNpgsql(cnnString)
                );
                break;

            default:
                services.AddDbContext<KhataContext>(options =>
                    options.UseSqlite(cnnString)
                );
                break;
        }

        services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<KhataContext>()
            .AddDefaultUI()
            .AddEntityFrameworkStores<KhataContext>();
        
        #region Register Entity Repositories
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
        #endregion
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        #region Register Reporting Repositories
        services.AddTransient<
            IIndividualReportRepository<CustomerReport>,
            CustomerReportRepository>();

        services.AddTransient<
            IIndividualReportRepository<SupplierReport>,
            SupplierReportRepository>();

        services.AddTransient<
            IReportRepository<Asset>,
            AssetReportRepository>();

        services.AddTransient<
            IReportRepository<Liability>,
            LiabilityReportRepository>();

        services.AddTransient<
            IReportRepository<PeriodicalReport<Inflow>>,
            InflowReportRepository>();

        services.AddTransient<
            IReportRepository<PeriodicalReport<Outflow>>,
            OutflowReportRepository>();

        services.AddTransient<
            IReportRepository<PeriodicalReport<Payable>>,
            PayableReportRepository>();

        services.AddTransient<
            IReportRepository<PeriodicalReport<Receivable>>,
            ReceivableReportRepository>();

        services.AddTransient<
            IListReportRepository<PerDayReport>,
            PerDayReportRepository>();

        services
            .AddTransient<
                IBackupRestoreRepository, 
                BackupRestoreRepository>();
        #endregion

        return services;
    }
}