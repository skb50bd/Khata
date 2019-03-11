using System.Collections.Generic;

using Queries;
using Business.Reports;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
{
    public class ReportModel : PageModel
    {
        public readonly IReportService<CustomerReport> CustomerReports;
        public ReportModel(IReportService<CustomerReport> customerReports)
        {
            CustomerReports = customerReports;
        }

        public IEnumerable<CustomerReport> Reports { get; set; }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Reports = await CustomerReports.Get();
        }
    }
}