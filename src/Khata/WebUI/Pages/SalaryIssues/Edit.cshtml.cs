using System.Threading.Tasks;

using AutoMapper;

using DTOs;
using Business.CRUD;
using ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.SalaryIssues
{
    public class EditModel : PageModel
    {
        private readonly ISalaryIssueService _salaryIssues;
        private readonly IMapper _mapper;

        public EditModel(ISalaryIssueService salaryIssues, IMapper mapper)
        {
            _salaryIssues = salaryIssues;
            _mapper = mapper;
        }

        [BindProperty]
        public SalaryIssueViewModel SalaryIssueVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryIssue = await _salaryIssues.Get((int)id);

            if (salaryIssue == null)
            {
                return NotFound();
            }

            SalaryIssueVm = _mapper.Map<SalaryIssueViewModel>(salaryIssue);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SalaryIssueDto dto;
            try
            {
                dto = await _salaryIssues.Update(SalaryIssueVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SalaryIssueExists(SalaryIssueVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Debt Issue: {dto.Id} - {dto.EmployeeFullName} - {dto.Amount} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> SalaryIssueExists(int id)
        {
            return await _salaryIssues.Exists(id);
        }
    }
}
