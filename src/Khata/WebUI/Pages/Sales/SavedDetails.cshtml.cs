using System.Threading.Tasks;

using DTOs;
using Business.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Pages.Sales
{
    public class SavedDetailsModel : PageModel
    {
        private readonly ISaleService _sales;
        public SavedDetailsModel(ISaleService sales)
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

            Sale = await _sales.GetSaved((int)id);

            if (Sale == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
