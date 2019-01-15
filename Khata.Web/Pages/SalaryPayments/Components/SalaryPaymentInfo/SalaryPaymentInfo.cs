using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryPayments.Components.SalaryPaymentInfo
{
    public class SalaryPaymentInfo : ViewComponent
    {
        public IViewComponentResult Invoke(SalaryPaymentDto salaryPayment
            )
        {
            return View(nameof(SalaryPaymentInfo), salaryPayment);
        }

    }
}
