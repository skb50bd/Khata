using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;
using System.Linq;
using System.Collections.Generic;
using Business.Abstractions;

namespace WebUI.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _products;
        private readonly IOutletService _outlets;
        private readonly PfService _pfService;
        public IndexModel(
            PfService pfService,
            IOutletService outlets,
            IProductService products)
        {
            _pfService = pfService;
            _outlets = outlets;
            _products = products;
            Products = new PagedList<ProductDto>();
        }

        public IPagedList<ProductDto> Products { get; set; }
        public IEnumerable<OutletDto> Outlets { get; set; }
        public int CurrentOutletId { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion


        public async Task<IActionResult> OnGetAsync(
            [FromRoute]int? outletId,
            string searchString = "",
                int pageIndex = 1,
                int pageSize = 0
            )
        {
            Outlets = await _outlets.Get();
            outletId = outletId ?? 0;
            CurrentOutletId = (int)outletId;

            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Products = await _products.Get((int)outletId, Pf);
            return Page();
        }
    }
}
