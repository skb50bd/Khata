using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Pages.Purchases
{
    public class DetailsModel : PageModel
    {
        private readonly IPurchaseService _purchases;

        public DetailsModel(IPurchaseService purchases)
        {
            _purchases = purchases;
        }

        public PurchaseDto Purchase { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Purchase = await _purchases.Get((int)id);

            if (Purchase == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetBriefAsync([FromQuery]int id)
        {
            var purchase = await _purchases.Get(id);

            if (purchase is null)
            {
                return NotFound();
            }
            return new PartialViewResult
            {
                ViewName = "_PurchaseBriefing",
                ViewData = new ViewDataDictionary<PurchaseDto>(ViewData, purchase)
            };
        }
    }
}
