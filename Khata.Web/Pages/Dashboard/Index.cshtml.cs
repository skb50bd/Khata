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
        public readonly IReportService<DailyIncomeReport> _dailyIncomeReports;
        public readonly IReportService<WeeklyIncomeReport> _weeklyIncomeReports;
        public readonly IReportService<MonthlyIncomeReport> _monthlyIncomeReports;

        public AssetReport AssetReport { get; set; }
        public LiabilityReport LiabilityReport { get; set; }
        public IncomeReports IncomeReports { get; set; }


        public IndexModel(IReportService<AssetReport> assetReports,
            IReportService<LiabilityReport> liabilityReports,
            IReportService<DailyIncomeReport> dailyIncomeReports,
            IReportService<WeeklyIncomeReport> weeklyIncomeReports,
            IReportService<MonthlyIncomeReport> monthlyIncomeReports)
        {
            _assetReports = assetReports;
            _liabilityReports = liabilityReports;
            _dailyIncomeReports = dailyIncomeReports;
            _weeklyIncomeReports = weeklyIncomeReports;
            _monthlyIncomeReports = monthlyIncomeReports;
        }

        public async Task OnGetAsync()
        {
            AssetReport = (await _assetReports.Get()).First();
            LiabilityReport = (await _liabilityReports.Get()).First();
            IncomeReports = new IncomeReports
            {
                Daily = (await _dailyIncomeReports.Get()).First(),
                Weekly = (await _weeklyIncomeReports.Get()).First(),
                Monthly = (await _monthlyIncomeReports.Get()).First()
            };
        }
    }
}