using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryIssues.Components.SalaryIssueForm
{
    public class SalaryIssueForm : ViewComponent
    {
        public IViewComponentResult Invoke(SalaryIssueViewModel salaryIssue)
        {
            return View(nameof(SalaryIssueForm), salaryIssue);
        }
    }
}
