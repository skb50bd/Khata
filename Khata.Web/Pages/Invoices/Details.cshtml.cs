using System.Threading.Tasks;

using Khata.Domain;
using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebUI.Pages.Invoices
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerInvoiceService _invoices;
        public OutletOptions Outlet { get; set; }

        public DetailsModel(
            ICustomerInvoiceService invoices,
            IOptionsMonitor<OutletOptions> options)
        {
            Outlet = options.CurrentValue;
            _invoices = invoices;
        }

        public CustomerInvoiceDto Invoice;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Invoice = await _invoices.Get(id);

            if (Invoice == null)
                return NotFound();

            return Page();
        }
    }
}
