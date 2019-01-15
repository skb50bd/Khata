using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SalaryPayments.Components.SalaryPaymentsList
{
    public class SalaryPaymentsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SalaryPaymentDto> salaryPayments)
        {
            return View(nameof(SalaryPaymentsList), salaryPayments);
        }
    }
}
