using System.Threading.Tasks;

using AutoMapper;

using DTOs;
using Business.CRUD;
using ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.SalaryPayments
{
    public class EditModel : PageModel
    {
        private readonly ISalaryPaymentService _salaryPayments;
        private readonly IMapper _mapper;

        public EditModel(ISalaryPaymentService salaryPayments, IMapper mapper)
        {
            _salaryPayments = salaryPayments;
            _mapper = mapper;
        }

        [BindProperty]
        public SalaryPaymentViewModel SalaryPaymentVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryPayment = await _salaryPayments.Get((int)id);

            if (salaryPayment == null)
            {
                return NotFound();
            }

            SalaryPaymentVm = _mapper.Map<SalaryPaymentViewModel>(salaryPayment);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SalaryPaymentDto dto;
            try
            {
                dto = await _salaryPayments.Update(SalaryPaymentVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SalaryPaymentExists(SalaryPaymentVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Debt Payment: {dto.Id} - {dto.EmployeeFullName} - {dto.Amount} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> SalaryPaymentExists(int id)
        {
            return await _salaryPayments.Exists(id);
        }
    }
}
