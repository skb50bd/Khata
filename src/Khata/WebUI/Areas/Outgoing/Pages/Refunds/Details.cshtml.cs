using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outgoing.Pages.Refunds;

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

        Refund = await _refunds.Get((int)id);

        if (Refund == null)
        {
            return NotFound();
        }

        return Page();
    }
}