using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerService _customers;
        public DetailsModel(ICustomerService customers)
        {
            _customers = customers;
        }

        public CustomerDto Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Customer = await _customers.Get((int)id);

            if (Customer is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
