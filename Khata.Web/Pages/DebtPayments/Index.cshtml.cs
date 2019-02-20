using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;

namespace WebUI.Pages.DebtPayments
{
    public class IndexModel : PageModel
    {
        private readonly IDebtPaymentService _debtPayments;
        private readonly PfService _pfService;
        public IndexModel(IDebtPaymentService debtPayments, PfService pfService)
        {
            _debtPayments = debtPayments;
            _pfService = pfService;
            DebtPayments = new PagedList<DebtPaymentDto>();
        }

        public IPagedList<DebtPaymentDto> DebtPayments { get; set; }
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
            DebtPayments = await _debtPayments.Get(Pf);
            return Page();
        }
    }
}
