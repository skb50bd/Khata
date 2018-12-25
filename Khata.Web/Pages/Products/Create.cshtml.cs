using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Products
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public CreateModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            ProductVM = new ProductViewModel();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductViewModel ProductVM { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var product = _mapper.Map<Product>(ProductVM);
            var inventory = product.Inventory;
            product.Metadata = Metadata.CreatedNew(User.Identity.Name);
            _db.Products.Add(product);
            await _db.CompleteAsync();

            Message = $"Product: {product.Id} - {product.Name} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}