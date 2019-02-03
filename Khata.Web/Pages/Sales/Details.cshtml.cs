using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Sales
{
    public class DetailsModel : PageModel
    {
        private readonly ISaleService _sales;
        public DetailsModel(ISaleService sales)
        {
            _sales = sales;
        }

        public SaleDto Sale { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale = await _sales.Get((int)id);

            if (Sale == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
