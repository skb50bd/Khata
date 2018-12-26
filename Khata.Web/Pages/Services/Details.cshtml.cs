using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Services
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

        public ServiceDto Service { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _db.Services.GetById((int)id);

            if (service == null)
            {
                return NotFound();
            }

            Service = _mapper.Map<ServiceDto>(service);
            return Page();
        }
    }
}
