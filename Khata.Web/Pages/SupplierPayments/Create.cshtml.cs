using System.Threading.Tasks;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.SupplierPayments
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISupplierPaymentService _supplierPayments;

        public CreateModel(ISupplierPaymentService supplierPayments)
        {
            _supplierPayments = supplierPayments;
        }

        public IActionResult OnGet()
        {
            SupplierPaymentVm = new SupplierPaymentViewModel();
            return Page();
        }

        [BindProperty]
        public SupplierPaymentViewModel SupplierPaymentVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var supplierPayment = await _supplierPayments.Add(SupplierPaymentVm);

            Message = $"Supplier Payment: {supplierPayment.Id} - {supplierPayment.SupplierFullName} - {supplierPayment.Amount} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}
