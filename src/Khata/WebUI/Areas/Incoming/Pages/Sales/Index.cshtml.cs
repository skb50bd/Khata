using System.Collections.Generic;
using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Incoming.Pages.Sales;

public class IndexModel : PageModel
{
    private readonly IOutletService _outlets;
    private readonly ISaleService _sales;
    private readonly PfService _pfService;
    public IndexModel(
        PfService pfService, 
        IOutletService outlets,
        ISaleService sales)
    {
        _pfService = pfService;
        _outlets = outlets;
        _sales = sales;
        Sales = new PagedList<SaleDto>();
    }

    public IPagedList<SaleDto> Sales { get; set; }
    public IEnumerable<OutletDto> Outlets { get; set; }
    public int CurrentOutletId { get; set; }
    public PageFilter Pf { get; set; }

    #region TempData
    [TempData]
    public string Message { get; set; }

    [TempData]
    public string MessageType { get; set; }
    #endregion

    public async Task<IActionResult> OnGetAsync(
        int? outletId,
        string searchString = "",
        int pageSize = 0,
        int pageIndex = 1)
    {
        Outlets = await _outlets.Get();
        outletId = outletId ?? 0;
        CurrentOutletId = (int)outletId;

        Pf    = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
        Sales = await _sales.Get((int)outletId, Pf);
        return Page();
    }
}