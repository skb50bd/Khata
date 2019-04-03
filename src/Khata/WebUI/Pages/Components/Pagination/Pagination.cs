
using Brotal;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Pagination
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel model) =>
            View(nameof(Pagination), model);
    }
}
