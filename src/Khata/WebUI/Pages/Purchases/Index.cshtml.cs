using System.Threading.Tasks;

using DTOs;
using Business.CRUD;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;

namespace WebUI.Pages.Purchases
{
    public class IndexModel : PageModel
    {
        private readonly IPurchaseService _purchases;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, IPurchaseService purchases)
        {
            _pfService = pfService;
            _purchases = purchases;
            Purchases = new PagedList<PurchaseDto>();
        }

        public IPagedList<PurchaseDto> Purchases { get; set; }
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
            Purchases = await _purchases.Get(Pf);
            return Page();
        }
    }
}
