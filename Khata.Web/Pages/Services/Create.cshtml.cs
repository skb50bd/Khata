using System.Threading.Tasks;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Services
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IServiceService _services;

        public CreateModel(IServiceService services)
        {
            _services = services;
            ServiceVm = new ServiceViewModel();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ServiceViewModel ServiceVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dto = await _services.Add(ServiceVm);

            Message = $"Service: {dto.Id} - {dto.Name} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}