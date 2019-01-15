using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryPayments.Components.SalaryPaymentForm
{
    public class SalaryPaymentForm : ViewComponent
    {
        public IViewComponentResult Invoke(SalaryPaymentViewModel salaryPayment)
        {
            return View(nameof(SalaryPaymentForm), salaryPayment);
        }
    }
}
