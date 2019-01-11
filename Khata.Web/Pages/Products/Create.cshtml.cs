using System.Threading.Tasks;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProductService _products;
        public CreateModel(IProductService products)
        {
            ProductVm = new ProductViewModel();
            _products = products;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductViewModel ProductVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var product = await _products.Add(ProductVm);

            Message = $"Product: {product.Id} - {product.Name} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}