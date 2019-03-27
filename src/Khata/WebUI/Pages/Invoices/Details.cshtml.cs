using System.Threading.Tasks;
using Business.Abstractions;
using Domain;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebUI.Pages.Invoices
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerInvoiceService _invoices;

        public DetailsModel(ICustomerInvoiceService invoices)
        {
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
