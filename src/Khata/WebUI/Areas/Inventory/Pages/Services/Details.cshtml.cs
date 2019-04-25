using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Inventory.Pages.Services
{
    public class DetailsModel : PageModel
    {
        private readonly IServiceService _services;

        public DetailsModel(IServiceService services)
        {
            _services = services;
        }

        public ServiceDto Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Service = await _services.Get((int)id);

            if (Service is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
