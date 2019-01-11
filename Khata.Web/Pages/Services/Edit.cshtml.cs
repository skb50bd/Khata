using System.Threading.Tasks;

using AutoMapper;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Services
{
    public class EditModel : PageModel
    {
        private readonly IServiceService _services;
        private readonly IMapper _mapper;

        public EditModel(IServiceService services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        [BindProperty]
        public ServiceViewModel ServiceVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceVm = _mapper.Map<ServiceViewModel>(
                await _services.Get((int)id));

            if (ServiceVm == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var service = await _services.Update(ServiceVm);

            Message = $"Service: {service.Id} - {service.Name} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }
    }
}
