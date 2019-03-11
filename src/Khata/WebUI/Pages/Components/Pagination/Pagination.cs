
using Microsoft.AspNetCore.Mvc;

using Brotal.Extensions;

namespace WebUI.Pages.Components.Pagination
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationModel model) =>
            View(nameof(Pagination), model);
    }
}
