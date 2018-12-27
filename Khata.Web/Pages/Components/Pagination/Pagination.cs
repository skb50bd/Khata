using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Pagination
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(Sieve model)
        {
            return View(nameof(Pagination), model);
        }
    }
}
