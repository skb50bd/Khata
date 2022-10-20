using System.Threading.Tasks;

using AutoMapper;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using ViewModels;

namespace WebUI.Areas.Outlets.Pages;

public class EditModel : PageModel
{
    private readonly IOutletService _outlets;
    private readonly IMapper _mapper;

    public EditModel(IOutletService outlets, IMapper mapper)
    {
        _outlets = outlets;
        _mapper = mapper;
    }

    [BindProperty]
    public OutletViewModel OutletVm { get; set; }

    [TempData] public string Message { get; set; }
    [TempData] public string MessageType { get; set; }


    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var outlet = await _outlets.Get((int)id);

        if (outlet is null)
        {
            return NotFound();
        }

        OutletVm = _mapper.Map<OutletViewModel>(outlet);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        OutletDto outlet;

        try
        {
            outlet = await _outlets.Update(OutletVm);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OutletExists(OutletVm.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        Message = $"Outlet: {outlet.Id} - {outlet.Title} updated!";
        MessageType = "success";

        return RedirectToPage("./Index");
    }

    private async Task<bool> OutletExists(int id)
    {
        return await _outlets.Exists(id);
    }
}