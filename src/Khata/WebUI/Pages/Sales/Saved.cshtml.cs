using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;
using System.Collections.Generic;
using Business.Abstractions;

namespace WebUI.Pages.Sales
{
    public class SavedModel : PageModel
    {
        private readonly ISaleService _sales;
        public SavedModel(ISaleService sales)
        {
            _sales = sales;
            Sales = new List<SaleDto>();
        }

        public IEnumerable<SaleDto> Sales { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync()
        {
            Sales = await _sales.GetSaved();
            return Page();
        }
    }
}
