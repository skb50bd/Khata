using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Refunds.Components.RefundsList
{
    public class RefundsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<RefundDto> refunds)
        {
            return View(nameof(RefundsList), refunds);
        }
    }
}
