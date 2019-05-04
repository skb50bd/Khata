using Domain.Reports;

using Microsoft.Extensions.DependencyInjection;

namespace Business.Reports
{
    public static class ReportConfiguration
    {
        public static IServiceCollection AddReports(
            this IServiceCollection services)
        {
            services.AddTransient<
                IIndividualReportService<CustomerReport>, 
                IndividualReportService<CustomerReport>>();

            services.AddTransient<
                IIndividualReportService<SupplierReport>, 
                IndividualReportService<SupplierReport>>();

            services.AddTransient<
                IReportService<Asset>, 
                ReportService<Asset>>();

            services.AddTransient<
                IReportService<Liability>, 
                ReportService<Liability>>();

            services.AddTransient<
                IReportService<PeriodicalReport<Inflow>>, 
                ReportService<PeriodicalReport<Inflow>>>();

            services.AddTransient<
                IReportService<PeriodicalReport<Outflow>>, 
                ReportService<PeriodicalReport<Outflow>>>();

            services.AddTransient<
                IReportService<PeriodicalReport<Payable>>,
                ReportService<PeriodicalReport<Payable>>>();

            services.AddTransient<
                IReportService<PeriodicalReport<Receivable>>,
                ReportService<PeriodicalReport<Receivable>>>();

            services.AddTransient<
                IListReportService<PerDayReport>, 
                ListReportService<PerDayReport>>();

            services.AddTransient<
                ISendEmailReport,
                SendEmailReport>();

            return services;
        }
    }
}
