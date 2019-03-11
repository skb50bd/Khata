using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.DebtPayments.Components.DebtPaymentForm
{
    public class DebtPaymentForm : ViewComponent
    {
        public IViewComponentResult Invoke(DebtPaymentViewModel debtPayment)
        {
            return View(nameof(DebtPaymentForm), debtPayment);
        }
    }
}
