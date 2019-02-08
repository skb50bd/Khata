using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.PurchaseReturns
{
    public class DetailsModel : PageModel
    {
        private readonly IPurchaseReturnService _purchaseReturns;
        public DetailsModel(IPurchaseReturnService purchaseReturns)
        {
            _purchaseReturns = purchaseReturns;
        }

        public PurchaseReturnDto PurchaseReturn { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            PurchaseReturn = await _purchaseReturns.Get((int)id);

            if (PurchaseReturn == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
