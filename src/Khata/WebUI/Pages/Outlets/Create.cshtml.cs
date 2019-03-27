using System.Threading.Tasks;
using Business.Abstractions;
using ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Outlets
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IOutletService _outlets;
        public CreateModel(IOutletService outlets)
        {
            OutletVm = new OutletViewModel();
            _outlets = outlets;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public OutletViewModel OutletVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var outlet = await _outlets.Add(OutletVm);

            Message = $"Outlet: {outlet.Id} - {outlet.Title} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}