using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Search
{
    public class Search : ViewComponent
    {
        public string PlaceHolder { get; set; }
        public IViewComponentResult Invoke(Sieve model)
        {
            return View(nameof(Search), model);
        }
    }
}
