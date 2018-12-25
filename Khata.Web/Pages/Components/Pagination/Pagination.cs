using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Components.Pagination
{
    public class Pagination : ViewComponent
    {
        public IViewComponentResult Invoke(int pageIndex, int pageSize, int totalPages, string currentFilter)
        {
            var pm = new PaginationModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPages,
                CurrentFilter = currentFilter
            };
            return View("Default", pm);
        }
    }
}
