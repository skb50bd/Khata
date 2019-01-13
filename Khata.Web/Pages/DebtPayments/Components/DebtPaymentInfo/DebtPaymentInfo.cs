using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.DebtPayments.Components.DebtPaymentInfo
{
    public class DebtPaymentInfo : ViewComponent
    {
        public IViewComponentResult Invoke(DebtPaymentDto debtPayment
            )
        {
            return View(nameof(DebtPaymentInfo), debtPayment);
        }

    }
}
