using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Inventory.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductService _products;
        public DetailsModel(IProductService products)
        {
            _products = products;
        }

        public ProductDto Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Product = await _products.Get((int)id);

            if (Product is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
