using System.Threading.Tasks;

using AutoMapper;
using Business.Abstractions;
using DTOs;
using ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.DebtPayments
{
    public class EditModel : PageModel
    {
        private readonly IDebtPaymentService _debtPayments;
        private readonly IMapper _mapper;

        public EditModel(IDebtPaymentService debtPayments, IMapper mapper)
        {
            _debtPayments = debtPayments;
            _mapper = mapper;
        }

        [BindProperty]
        public DebtPaymentViewModel DebtPaymentVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debtPayment = await _debtPayments.Get((int)id);

            if (debtPayment == null)
            {
                return NotFound();
            }

            DebtPaymentVm = _mapper.Map<DebtPaymentViewModel>(debtPayment);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DebtPaymentDto dto;
            try
            {
                dto = await _debtPayments.Update(DebtPaymentVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DebtPaymentExists(DebtPaymentVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Debt Payment: {dto.Id} - {dto.CustomerFullName} - {dto.Amount} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> DebtPaymentExists(int id)
        {
            return await _debtPayments.Exists(id);
        }
    }
}
