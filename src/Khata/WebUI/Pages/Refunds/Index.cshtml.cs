using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Refunds
{
    public class IndexModel : PageModel
    {
        private readonly IRefundService _refunds;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, IRefundService refunds)
        {
            _pfService = pfService;
            _refunds = refunds;
            Refunds = new PagedList<RefundDto>();
        }

        public IPagedList<RefundDto> Refunds { get; set; }
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
            Refunds = await _refunds.Get(Pf);
            return Page();
        }
    }
}
