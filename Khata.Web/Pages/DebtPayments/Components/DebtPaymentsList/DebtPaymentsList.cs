using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.DebtPayments.Components.DebtPaymentsList
{
    public class DebtPaymentsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<DebtPaymentDto> debtPayments)
        {
            return View(nameof(DebtPaymentsList), debtPayments);
        }
    }
}
