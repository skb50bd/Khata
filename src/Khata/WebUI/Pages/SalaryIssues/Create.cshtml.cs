using System.Threading.Tasks;
using Business.Abstractions;
using ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.SalaryIssues
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISalaryIssueService _salaryIssues;

        public CreateModel(ISalaryIssueService salaryIssues)
        {
            _salaryIssues = salaryIssues;
        }

        public IActionResult OnGet()
        {
            SalaryIssueVm = new SalaryIssueViewModel();
            return Page();
        }

        [BindProperty]
        public SalaryIssueViewModel SalaryIssueVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var salaryIssue = await _salaryIssues.Add(SalaryIssueVm);

            Message = $"Debt Issue: {salaryIssue.Id} - {salaryIssue.EmployeeFullName} - {salaryIssue.Amount} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}
