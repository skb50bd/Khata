using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
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

        public CustomerDto Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers.GetById((int)id);

            if (customer == null)
            {
                return NotFound();
            }

            Customer = _mapper.Map<CustomerDto>(customer);
            return Page();
        }
    }
}
