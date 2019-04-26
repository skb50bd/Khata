using System.Collections.Generic;
using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outlets.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IOutletService _outlets;
        public IndexModel(IOutletService outlets)
        {
            _outlets = outlets;
            Outlets = new List<OutletDto>();
        }

        public IEnumerable<OutletDto> Outlets { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion


        public async Task<IActionResult> OnGetAsync()
        {
            Outlets = await _outlets.Get();
            return Page();
        }
    }
}
