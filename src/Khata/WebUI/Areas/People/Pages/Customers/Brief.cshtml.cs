using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Areas.People.Pages.Customers
{
    public class BriefModel : PageModel
    {
        private readonly ICustomerService _customers;
        public BriefModel(ICustomerService customers)
        {
            _customers = customers;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!await _customers.Exists((int)id))
            {
                return NotFound();
            }
            var customer = await _customers.Get((int)id);

            return new PartialViewResult
            {
                ViewName = "_CustomerBriefInfo",
                ViewData = new ViewDataDictionary<CustomerDto>(ViewData, customer)
            };
        }
    }
}
