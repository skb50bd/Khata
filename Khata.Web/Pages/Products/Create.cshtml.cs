using System.Threading.Tasks;

using AutoMapper;

using Khata.Domain;
using Khata.Domain.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _4._Web.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly Khata.Data.KhataContext _context;
        private readonly IMapper _mapper;

        public CreateModel(Khata.Data.KhataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Product = new ProductViewModel();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductViewModel Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var product = _mapper.Map<Product>(Product);
            var inventory = product.Inventory;
            product.Metadata = Metadata.CreatedNew(User.Identity.Name);
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}