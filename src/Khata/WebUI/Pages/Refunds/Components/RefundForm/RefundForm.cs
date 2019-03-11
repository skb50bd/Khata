using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Refunds.Components.RefundForm
{
    public class RefundForm : ViewComponent
    {
        public IViewComponentResult Invoke(RefundViewModel refund)
        {
            return View(nameof(RefundForm), refund);
        }
    }
}
