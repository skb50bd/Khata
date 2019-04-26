using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outgoing.Pages.SalaryIssues
{
    public class DetailsModel : PageModel
    {
        private readonly ISalaryIssueService _salaryIssues;
        public DetailsModel(ISalaryIssueService salaryIssues)
        {
            _salaryIssues = salaryIssues;
        }

        public SalaryIssueDto SalaryIssue { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalaryIssue = await _salaryIssues.Get((int)id);

            if (SalaryIssue == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
