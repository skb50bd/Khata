using System.Linq;
using System.Threading.Tasks;

using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs
{
    public class ReportsHub : Hub
    {
        private readonly IReportService<CustomerReport> _customerReports;
        private readonly IReportService<AssetReport> _assetReport;
        private readonly IReportService<LiabilityReport> _liabilityReport;
        private readonly IReportService<DailyIncomeReport> _dailyIncomeReports;
        private readonly IReportService<WeeklyIncomeReport> _weeklyIncomeReports;
        private readonly IReportService<MonthlyIncomeReport> _monthlyIncomeReports;

        public AssetReport Asset { get; set; }
        public LiabilityReport Liability { get; set; }
        public IncomeReports Income { get; set; }

        public ReportsHub(
            IReportService<CustomerReport> customerReports,
            IReportService<AssetReport> assetReport,
            IReportService<LiabilityReport> liabilityReport,
            IReportService<DailyIncomeReport> dailyIncomeReports,
            IReportService<WeeklyIncomeReport> weeklyIncomeReports,
            IReportService<MonthlyIncomeReport> monthlyIncomeReports)
        {
            _customerReports = customerReports;
            _assetReport = assetReport;
            _liabilityReport = liabilityReport;
            _dailyIncomeReports = dailyIncomeReports;
            _weeklyIncomeReports = weeklyIncomeReports;
            _monthlyIncomeReports = monthlyIncomeReports;
        }

        public async Task UpdateChartData()
        {
            Asset = (await _assetReport.Get()).First();
            Liability = (await _liabilityReport.Get()).First();
            Income = new IncomeReports
            {
                Daily = (await _dailyIncomeReports.Get()).First(),
                Weekly = (await _weeklyIncomeReports.Get()).First(),
                Monthly = (await _monthlyIncomeReports.Get()).First()
            };
        }

        public async Task InitChartData()
        {
            await UpdateChartData();
            await Clients.All.SendAsync("UpdateChart", new { Asset, Liability, Income });
        }

        public async Task RefreshData() => await InitChartData();
    }
}
