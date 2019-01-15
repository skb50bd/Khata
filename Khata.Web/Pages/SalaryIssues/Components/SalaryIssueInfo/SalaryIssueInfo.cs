using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryIssues.Components.SalaryIssueInfo
{
    public class SalaryIssueInfo : ViewComponent
    {
        public IViewComponentResult Invoke(SalaryIssueDto salaryIssue
            )
        {
            return View(nameof(SalaryIssueInfo), salaryIssue);
        }

    }
}
