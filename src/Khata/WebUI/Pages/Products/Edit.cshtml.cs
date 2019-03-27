using System.Threading.Tasks;

using AutoMapper;
using Business.Abstractions;
using DTOs;
using ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain;

namespace WebUI.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductService _products;
        private readonly IOutletService _outlets;
        private readonly IMapper _mapper;

        public EditModel(
            IProductService products, 
            IOutletService outlets,
            IMapper mapper)
        {
            _products = products;
            _outlets = outlets;
            _mapper = mapper;
        }

        [BindProperty]
        public ProductViewModel ProductVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            ViewData["Outlets"] = new SelectList(
                await _outlets.Get(),
                nameof(Outlet.Id),
                nameof(Outlet.Title)
            );
            var product = await _products.Get((int)id);

            if (product is null)
            {
                return NotFound();
            }

            ProductVm = _mapper.Map<ProductViewModel>(product);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProductDto product;

            try
            {
                product = await _products.Update(ProductVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(ProductVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Product: {product.Id} - {product.Name} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _products.Exists(id);
        }
    }
}
