using System.Threading.Tasks;

using Business.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ViewModels;

namespace WebUI.Areas.People.Pages.Suppliers
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISupplierService _suppliers;

        public CreateModel(ISupplierService suppliers)
        {
            _suppliers = suppliers;
        }

        public IActionResult OnGet()
        {
            SupplierVm = new SupplierViewModel();
            return Page();
        }

        [BindProperty]
        public SupplierViewModel SupplierVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var supplier = await _suppliers.Add(SupplierVm);

            Message = $"Supplier: {supplier.Id} - {supplier.FullName} created!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }
    }
}