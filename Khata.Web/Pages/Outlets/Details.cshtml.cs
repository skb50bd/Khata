using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Outlets
{
    public class DetailsModel : PageModel
    {
        private readonly IOutletService _outlets;
        public DetailsModel(IOutletService outlets)
        {
            _outlets = outlets;
        }

        public OutletDto Outlet { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Outlet = await _outlets.Get((int)id);

            if (Outlet is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
