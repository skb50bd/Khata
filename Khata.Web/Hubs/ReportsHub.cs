using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs
{
    public class ReportsHub : Hub
    {
        private readonly IReportService<AssetReport> _assetReport;
        private readonly IReportService<LiabilityReport> _liabilityReport;
        private readonly IReportService<DailyIncomeReport> _dailyIncomeReports;
        private readonly IReportService<WeeklyIncomeReport> _weeklyIncomeReports;
        private readonly IReportService<MonthlyIncomeReport> _monthlyIncomeReports;
        private readonly IReportService<DailyExpenseReport> _dailyExpenseReports;
        private readonly IReportService<WeeklyExpenseReport> _weeklyExpenseReports;
        private readonly IReportService<MonthlyExpenseReport> _monthlyExpenseReports;
        private readonly IReportService<PerDayReport> _perDayReports;


        public AssetReport Asset { get; set; }
        public LiabilityReport Liability { get; set; }
        public IncomeReports Income { get; set; }
        public ExpenseReports Expense { get; set; }
        public IEnumerable<PerDayReport> PerDayReports { get; set; }

        public ReportsHub(
            IReportService<AssetReport> assetReport,
            IReportService<LiabilityReport> liabilityReport,
            IReportService<DailyIncomeReport> dailyIncomeReports,
            IReportService<WeeklyIncomeReport> weeklyIncomeReports,
            IReportService<MonthlyIncomeReport> monthlyIncomeReports,
            IReportService<DailyExpenseReport> dailyExpenseReports,
            IReportService<WeeklyExpenseReport> weeklyExpenseReports,
            IReportService<MonthlyExpenseReport> monthlyExpenseReports,
            IReportService<PerDayReport> perDayReports)
        {
            _assetReport = assetReport;
            _liabilityReport = liabilityReport;
            _dailyIncomeReports = dailyIncomeReports;
            _weeklyIncomeReports = weeklyIncomeReports;
            _monthlyIncomeReports = monthlyIncomeReports;
            _dailyExpenseReports = dailyExpenseReports;
            _weeklyExpenseReports = weeklyExpenseReports;
            _monthlyExpenseReports = monthlyExpenseReports;
            _perDayReports = perDayReports;
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

            Expense = new ExpenseReports
            {
                Daily = (await _dailyExpenseReports.Get()).First(),
                Weekly = (await _weeklyExpenseReports.Get()).First(),
                Monthly = (await _monthlyExpenseReports.Get()).First()
            };

            PerDayReports = await _perDayReports.Get();
        }

        public async Task InitChartData()
        {
            await UpdateChartData();
            await Clients.All.SendAsync("UpdateChart", new { Asset, Liability, Income, Expense, PerDayReports });
        }

        public async Task RefreshData() => await InitChartData();
    }
}
