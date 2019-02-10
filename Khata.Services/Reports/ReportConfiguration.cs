using Khata.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Khata.Services.Reports
{
    public static class ReportConfiguration
    {
        public static IServiceCollection AddReports(
            this IServiceCollection services)
        {
            services.AddTransient<IReportService<CustomerReport>, ReportService<CustomerReport>>();
            services.AddTransient<IReportService<SupplierReport>, ReportService<SupplierReport>>();
            services.AddTransient<IReportService<AssetReport>, ReportService<AssetReport>>();
            services.AddTransient<IReportService<LiabilityReport>, ReportService<LiabilityReport>>();
            services.AddTransient<IReportService<DailyIncomeReport>, ReportService<DailyIncomeReport>>();
            services.AddTransient<IReportService<WeeklyIncomeReport>, ReportService<WeeklyIncomeReport>>();
            services.AddTransient<IReportService<MonthlyIncomeReport>, ReportService<MonthlyIncomeReport>>();

            return services;
        }
    }
}
