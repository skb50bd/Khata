using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        public DetailsModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public ProductDto Product { get; set; }

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

            Product = _mapper.Map<ProductDto>(product);
            return Page();
        }
    }
}
