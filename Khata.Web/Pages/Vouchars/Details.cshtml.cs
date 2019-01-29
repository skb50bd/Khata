using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Khata.Domain;
using Khata.Services.CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Vouchars
{
    public class DetailsModel : PageModel
    {
        private readonly IVoucharService _vouchars;
        public DetailsModel(IVoucharService vouchars)
        {
            _vouchars = vouchars;
        }

        public Vouchar Vouchar;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vouchar = await _vouchars.Get(id);

            if (Vouchar == null)
                return NotFound();

            return Page();
        }
    }
}
