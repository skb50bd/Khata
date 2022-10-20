using System.Linq;
using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Incoming.Pages.Invoices;

public class DetailsModel : PageModel
{
    private readonly ICustomerInvoiceService _invoices;
    private readonly IOutletService _outlets;

    public DetailsModel(
        ICustomerInvoiceService invoices, 
        IOutletService outlets)
    {
        _invoices = invoices;
        _outlets = outlets;
    }

    public CustomerInvoiceDto Invoice;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Invoice = await _invoices.Get(id);

        if (Invoice == null)
            return NotFound();

        if (Invoice.Outlet is null)
            Invoice.Outlet = (await _outlets.Get()).FirstOrDefault();

        return Page();
    }
}