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

            return services;
        }
    }
}
