using System.Threading.Tasks;

using Domain;
using DTOs;
using Business.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebUI.Pages.Vouchars
{
    public class DetailsModel : PageModel
    {
        private readonly IVoucharService _vouchars;
        public DetailsModel(IVoucharService vouchars, IOptionsMonitor<OutletOptions> options)
        {
            _vouchars = vouchars;
            Options = options.CurrentValue;
        }

        public VoucharDto Vouchar;
        public OutletOptions Options;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vouchar = await _vouchars.Get(id);

            if (Vouchar == null)
                return NotFound();

            return Page();
        }
    }
}
