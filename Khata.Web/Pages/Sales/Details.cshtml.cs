using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Sales
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

        public SaleDto Sale { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _db.Sales.GetById((int)id);

            if (sale == null)
            {
                return NotFound();
            }

            Sale = _mapper.Map<SaleDto>(sale);
            return Page();
        }
    }
}
