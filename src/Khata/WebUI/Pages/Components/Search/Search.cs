
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Search
{
    public class Search : ViewComponent
    {
        public IViewComponentResult Invoke(PageFilter model)
        {
            model.Filter = model.Filter ?? "";
            return View(nameof(Search), model);
        }
    }
}
