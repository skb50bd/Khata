using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Refunds
{
    public class DetailsModel : PageModel
    {
        private readonly IRefundService _refunds;
        public DetailsModel(IRefundService refunds)
        {
            _refunds = refunds;
        }

        public RefundDto Refund { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Refund = await _refunds.Get((int)id);

            if (Refund == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
