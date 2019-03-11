using System.Threading.Tasks;

using DTOs;
using Business.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.SalaryPayments
{
    public class DetailsModel : PageModel
    {
        private readonly ISalaryPaymentService _salaryPayments;
        public DetailsModel(ISalaryPaymentService salaryPayments)
        {
            _salaryPayments = salaryPayments;
        }

        public SalaryPaymentDto SalaryPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalaryPayment = await _salaryPayments.Get((int)id);

            if (SalaryPayment == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
