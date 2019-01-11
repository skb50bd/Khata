using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SharedLibrary;

namespace WebUI.Pages.Sales
{
    public class IndexModel : PageModel
    {
        private readonly ISaleService _sales;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, ISaleService sales)
        {
            _pfService = pfService;
            _sales = sales;
            Sales = new PagedList<SaleDto>();
        }

        public IPagedList<SaleDto> Sales { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Sales = await _sales.Get(Pf);
            return Page();
        }
    }
}
