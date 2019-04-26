using System.Threading.Tasks;

using Business.Abstractions;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using ViewModels;

namespace WebUI.Areas.Inventory.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IProductService _products;
        private readonly IOutletService _outlets;
        public CreateModel(
            IProductService products,
            IOutletService outlets)
        {
            ProductVm = new ProductViewModel();
            _products = products;
            _outlets = outlets;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Outlets"] = new SelectList(
                await _outlets.Get(), 
                nameof(Outlet.Id), 
                nameof(Outlet.Title)
            );
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