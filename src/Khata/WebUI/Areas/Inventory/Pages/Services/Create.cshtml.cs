using System.Threading.Tasks;

using Business.Abstractions;

using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using ViewModels;

namespace WebUI.Areas.Inventory.Pages.Services
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IServiceService _services;
        private readonly IOutletService _outlets;

        public CreateModel(IServiceService services,
            IOutletService outlets)
        {
            _services = services;
            _outlets = outlets;
            ServiceVm = new ServiceViewModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Outlets"] = new SelectList(
                await _outlets.Get(),
                nameof(Outlet.Id),
                nameof(Outlet.Title)
            );
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