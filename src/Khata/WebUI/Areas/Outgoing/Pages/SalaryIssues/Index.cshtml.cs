using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outgoing.Pages.SalaryIssues
{
    public class IndexModel : PageModel
    {
        private readonly ISalaryIssueService _salaryIssues;
        private readonly PfService _pfService;
        public IndexModel(ISalaryIssueService salaryIssues, PfService pfService)
        {
            _salaryIssues = salaryIssues;
            _pfService = pfService;
            SalaryIssues = new PagedList<SalaryIssueDto>();
        }

        public IPagedList<SalaryIssueDto> SalaryIssues { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            SalaryIssues = await _salaryIssues.Get(Pf);
            return Page();
        }
    }
}
