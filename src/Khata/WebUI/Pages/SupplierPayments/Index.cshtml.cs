using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;
using Business.Abstractions;

namespace WebUI.Pages.SupplierPayments
{
    public class IndexModel : PageModel
    {
        private readonly ISupplierPaymentService _supplierPayments;
        private readonly PfService _pfService;
        public IndexModel(ISupplierPaymentService supplierPayments, PfService pfService)
        {
            _supplierPayments = supplierPayments;
            _pfService = pfService;
            SupplierPayments = new PagedList<SupplierPaymentDto>();
        }

        public IPagedList<SupplierPaymentDto> SupplierPayments { get; set; }
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
            SupplierPayments = await _supplierPayments.Get(Pf);
            return Page();
        }
    }
}
