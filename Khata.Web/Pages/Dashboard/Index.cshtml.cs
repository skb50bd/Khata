using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IReportService<AssetReport> _assetReports;
        private readonly IReportService<LiabilityReport> _liabilityReports;
        private readonly IReportService<DailyIncomeReport> _dailyIncomeReports;
        private readonly IReportService<WeeklyIncomeReport> _weeklyIncomeReports;
        private readonly IReportService<MonthlyIncomeReport> _monthlyIncomeReports;
        private readonly IReportService<DailyExpenseReport> _dailyExpenseReports;
        private readonly IReportService<WeeklyExpenseReport> _weeklyExpenseReports;
        private readonly IReportService<MonthlyExpenseReport> _monthlyExpenseReports;
        private readonly IReportService<PerDayReport> _perDayReports;

        public AssetReport AssetReport { get; set; }
        public LiabilityReport LiabilityReport { get; set; }
        public IncomeReports IncomeReports { get; set; }
        public ExpenseReports ExpenseReports { get; set; }
        public IEnumerable<PerDayReport> PerDayReports { get; set; }


        public IndexModel(IReportService<AssetReport> assetReports,
            IReportService<LiabilityReport> liabilityReports,
            IReportService<DailyIncomeReport> dailyIncomeReports,
            IReportService<WeeklyIncomeReport> weeklyIncomeReports,
            IReportService<MonthlyIncomeReport> monthlyIncomeReports,
            IReportService<DailyExpenseReport> dailyExpenseReports,
            IReportService<WeeklyExpenseReport> weeklyExpenseReports,
            IReportService<MonthlyExpenseReport> monthlyExpenseReports,
            IReportService<PerDayReport> perDayReports)
        {
            _assetReports = assetReports;
            _liabilityReports = liabilityReports;
            _dailyIncomeReports = dailyIncomeReports;
            _weeklyIncomeReports = weeklyIncomeReports;
            _monthlyIncomeReports = monthlyIncomeReports;
            _dailyExpenseReports = dailyExpenseReports;
            _weeklyExpenseReports = weeklyExpenseReports;
            _monthlyExpenseReports = monthlyExpenseReports;
            _perDayReports = perDayReports;
        }

        public async Task OnGetAsync()
        {
            AssetReport = (await _assetReports.Get()).FirstOrDefault();
            LiabilityReport = (await _liabilityReports.Get()).FirstOrDefault();
            IncomeReports = new IncomeReports
            {
                Daily = (await _dailyIncomeReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyIncomeReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyIncomeReports.Get()).FirstOrDefault()
            };

            ExpenseReports = new ExpenseReports
            {
                Daily = (await _dailyExpenseReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyExpenseReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyExpenseReports.Get()).FirstOrDefault()
            };

            PerDayReports = await _perDayReports.Get();

            var c = PerDayReports;
        }
    }
}