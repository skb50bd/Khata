using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Pages.Customers;

[AllowAnonymous]
public class BriefInfoModel : PageModel
{
    private readonly ICustomerService _customers;

    public BriefInfoModel(ICustomerService customers)
    {
        _customers = customers;
    }

    public async Task<IActionResult> OnGetBriefAsync(int customerId)
    {
        if (!await _customers.Exists(customerId))
        {
            return NotFound();
        }
        var customer = await _customers.Get(customerId);
        return new PartialViewResult
        {
            ViewName = "_CustomerBriefInfo",
            ViewData = new ViewDataDictionary<CustomerDto>(ViewData, customer)
        };
    }
}