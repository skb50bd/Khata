
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Search
{
    public class Search : ViewComponent
    {
        public IViewComponentResult Invoke(PageFilter model) => 
            View(nameof(Search), model);
    }
}
