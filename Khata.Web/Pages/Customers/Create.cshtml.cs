using System.Threading.Tasks;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ICustomerService _customers;

        public CreateModel(ICustomerService customers)
        {
            _customers = customers;
        }

        public IActionResult OnGet()
        {
            CustomerVm = new CustomerViewModel();
            return Page();
        }

        [BindProperty]
        public CustomerViewModel CustomerVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var customer = await _customers.Add(CustomerVm);

            Message = $"Customer: {customer.Id} - {customer.FullName} created!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }
    }
}