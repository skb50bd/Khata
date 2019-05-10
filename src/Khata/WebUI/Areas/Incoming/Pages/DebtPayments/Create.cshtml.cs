using System.Threading.Tasks;

using Business.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ViewModels;

namespace WebUI.Areas.Incoming.Pages.DebtPayments
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IDebtPaymentService _debtPayments;

        public CreateModel(IDebtPaymentService debtPayments)
        {
            _debtPayments = debtPayments;
        }

        public IActionResult OnGet()
        {
            DebtPaymentVm = new DebtPaymentViewModel();
            return Page();
        }

        [BindProperty]
        public DebtPaymentViewModel DebtPaymentVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var debtPayment = await _debtPayments.Add(DebtPaymentVm);

            Message = $"Debt-Payment: {debtPayment.Id} - {debtPayment.CustomerFullName} - {debtPayment.Amount} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}
