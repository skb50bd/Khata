using Khata.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.Reports
{
    public static class ReportConfiguration
    {
        public static IServiceCollection AddReports(
            this IServiceCollection services)
        {
            services.AddTransient<IIndividualReportService<CustomerReport>, IndividualReportService<CustomerReport>>();
            services.AddTransient<IIndividualReportService<SupplierReport>, IndividualReportService<SupplierReport>>();
            services.AddTransient<IReportService<AssetReport>, ReportService<AssetReport>>();
            services.AddTransient<IReportService<LiabilityReport>, ReportService<LiabilityReport>>();
            services.AddTransient<IReportService<DailyIncomeReport>, ReportService<DailyIncomeReport>>();
            services.AddTransient<IReportService<DailyExpenseReport>, ReportService<DailyExpenseReport>>();
            services.AddTransient<IReportService<DailyPayableReport>, ReportService<DailyPayableReport>>();
            services.AddTransient<IReportService<DailyReceivableReport>, ReportService<DailyReceivableReport>>();
            services.AddTransient<IReportService<DailyOutletSalesReport>, ReportService<DailyOutletSalesReport>>();
            services.AddTransient<IReportService<WeeklyIncomeReport>, ReportService<WeeklyIncomeReport>>();
            services.AddTransient<IReportService<WeeklyExpenseReport>, ReportService<WeeklyExpenseReport>>();
            services.AddTransient<IReportService<WeeklyPayableReport>, ReportService<WeeklyPayableReport>>();
            services.AddTransient<IReportService<WeeklyReceivableReport>, ReportService<WeeklyReceivableReport>>();
            services.AddTransient<IReportService<WeeklyOutletSalesReport>, ReportService<WeeklyOutletSalesReport>>();
            services.AddTransient<IReportService<MonthlyIncomeReport>, ReportService<MonthlyIncomeReport>>();
            services.AddTransient<IReportService<MonthlyExpenseReport>, ReportService<MonthlyExpenseReport>>();
            services.AddTransient<IReportService<MonthlyPayableReport>, ReportService<MonthlyPayableReport>>();
            services.AddTransient<IReportService<MonthlyReceivableReport>, ReportService<MonthlyReceivableReport>>();
            services.AddTransient<IReportService<MonthlyOutletSalesReport>, ReportService<MonthlyOutletSalesReport>>();
            services.AddTransient<IReportService<PerDayReport>, ReportService<PerDayReport>>();

            return services;
        }
    }
}
