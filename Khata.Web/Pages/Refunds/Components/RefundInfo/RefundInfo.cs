using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Refunds.Components.RefundInfo
{
    public class RefundInfo : ViewComponent
    {
        public IViewComponentResult Invoke(RefundDto refund)
        {
            return View(nameof(RefundInfo), refund);
        }

    }
}
