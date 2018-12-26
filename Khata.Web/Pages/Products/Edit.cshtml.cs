using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using StonedExtensions;

namespace WebUI.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public EditModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [BindProperty]
        public ProductViewModel ProductVM { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _db.Products.GetById((int)id);

            if (product == null)
            {
                return NotFound();
            }

            ProductVM = _mapper.Map<ProductViewModel>(product);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newProduct = _mapper.Map<Product>(ProductVM);
            var originalProduct = await _db.Products.GetById(newProduct.Id);
            var meta = originalProduct.Metadata.Modified(User.Identity.Name);
            originalProduct.SetValuesFrom(newProduct);
            originalProduct.Metadata = meta;

            try
            {
                await _db.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(newProduct.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Product: {newProduct.Id} - {newProduct.Name} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _db.Products.Exists(id);
        }
    }
}
