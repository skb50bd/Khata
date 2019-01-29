using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SharedLibrary;

namespace WebUI.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _products;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, IProductService products)
        {
            _pfService = pfService;
            _products = products;
            Products = new PagedList<ProductDto>();
        }

        public IPagedList<ProductDto> Products { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion


        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
                int pageIndex = 1,
                int pageSize = 0
            )
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Products = await _products.Get(Pf);
            return Page();
        }
    }
}
