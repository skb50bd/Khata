using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs
{
    public class ReportsHub : Hub
    {
        public readonly IReportService<CustomerReport> CustomerReports;
        public ReportsHub(IReportService<CustomerReport> customerReports)
        {
            CustomerReports = customerReports;
        }
    }
}
