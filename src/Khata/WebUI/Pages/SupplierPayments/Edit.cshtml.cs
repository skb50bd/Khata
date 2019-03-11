using System.Threading.Tasks;

using AutoMapper;

using DTOs;
using Business.CRUD;
using ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.SupplierPayments
{
    public class EditModel : PageModel
    {
        private readonly ISupplierPaymentService _supplierPayments;
        private readonly IMapper _mapper;

        public EditModel(ISupplierPaymentService supplierPayments, IMapper mapper)
        {
            _supplierPayments = supplierPayments;
            _mapper = mapper;
        }

        [BindProperty]
        public SupplierPaymentViewModel SupplierPaymentVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplierPayment = await _supplierPayments.Get((int)id);

            if (supplierPayment == null)
            {
                return NotFound();
            }

            SupplierPaymentVm = _mapper.Map<SupplierPaymentViewModel>(supplierPayment);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SupplierPaymentDto dto;
            try
            {
                dto = await _supplierPayments.Update(SupplierPaymentVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SupplierPaymentExists(SupplierPaymentVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Supplier Payment: {dto.Id} - {dto.SupplierFullName} - {dto.Amount} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> SupplierPaymentExists(int id)
        {
            return await _supplierPayments.Exists(id);
        }
    }
}
