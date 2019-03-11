using System.Threading.Tasks;

using DTOs;
using Business.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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

        public async Task<IActionResult> OnGetBriefAsync([FromQuery]int id)
        {
            var sale = await _sales.Get(id);

            if (sale is null)
            {
                return NotFound();
            }
            return new PartialViewResult
            {
                ViewName = "_SaleBriefing",
                ViewData = new ViewDataDictionary<SaleDto>(ViewData, sale)
            };
        }
    }
}
