using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SharedLibrary;

namespace WebUI.Pages.Suppliers
{
    public class IndexModel : PageModel
    {
        private readonly ISupplierService _suppliers;
        private readonly PfService _pfService;
        public IndexModel(ISupplierService suppliers, PfService pfService)
        {
            _suppliers = suppliers;
            _pfService = pfService;
            Suppliers = new PagedList<SupplierDto>();
        }

        public IPagedList<SupplierDto> Suppliers { get; set; }
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
            Suppliers = await _suppliers.Get(Pf);
            return Page();
        }
    }
}
