using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Dashboard
{
    public class Index2Model : PageModel
    {
        private readonly IReportService<AssetReport> _assetReports;
        private readonly IReportService<LiabilityReport> _liabilityReport;
        private readonly IReportService<PerDayReport> _perDayReport;
        public Index2Model(IReportService<AssetReport> assetReports,
            IReportService<LiabilityReport> liabilityReport,
            IReportService<PerDayReport> perDayReport)
        {
            _assetReports = assetReports;
            _liabilityReport = liabilityReport;
            _perDayReport = perDayReport;
        }


        public AssetReport Asset { get; set; }
        public LiabilityReport Liability { get; set; }
        public PerDayReport PerDay { get; set; }

        public void OnGet()
        {

        }
    }
}