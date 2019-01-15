using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryIssues.Components.SalaryIssuesList
{
    public class SalaryIssuesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SalaryIssueDto> salaryIssues)
        {
            return View(nameof(SalaryIssuesList), salaryIssues);
        }
    }
}
