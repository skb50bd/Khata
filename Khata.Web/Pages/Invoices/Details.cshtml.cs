using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Khata.Domain;
using Khata.Services.CRUD;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Invoices
{
    public class DetailsModel : PageModel
    {
        private readonly IInvoiceService _invoices;
        public DetailsModel(IInvoiceService invoices)
        {
            _invoices = invoices;
        }

        public CustomerInvoice Invoice;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Invoice = await _invoices.Get(id);

            if (Invoice == null)
                return NotFound();

            return Page();
        }
    }
}
