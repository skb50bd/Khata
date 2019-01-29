using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Purchases
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

        public PurchaseDto Purchase { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _db.Purchases.GetById((int)id);

            if (purchase == null)
            {
                return NotFound();
            }

            Purchase = _mapper.Map<PurchaseDto>(purchase);
            return Page();
        }
    }
}
