using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Business.Abstractions;

using Domain;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using ViewModels;

namespace WebUI.Areas.Incoming.Pages.Sales
{
    [Authorize(Policy = "UserRights")]
    public class CreateModel : PageModel
    {
        private readonly ISaleService _sales;
        private readonly IOutletService _outlets;
        private readonly IMapper _mapper;

        public CreateModel(
            ISaleService sales,
            IOutletService outlets,
            IMapper mapper)
        {
            SaleVm = new SaleViewModel();
            _sales = sales;
            _outlets = outlets;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Outlets"] =
                new SelectList(
                    await _outlets.Get(),
                    nameof(Outlet.Id),
                    nameof(Outlet.Title)
                );
            return Page();
        }

        public async Task<IActionResult> OnGetSavedAsync(int id)
        {
            ViewData["Outlets"] =
                new SelectList(
                    await _outlets.Get(),
                    nameof(Outlet.Id),
                    nameof(Outlet.Title)
                );
            SaleVm = _mapper.Map<SaleViewModel>(await _sales.GetSaved(id));

            return Page();
        }

        [BindProperty]
        public SaleViewModel SaleVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                    Debug.WriteLine(e.ErrorMessage);
                return Page();
            }

            SaleDto sale;
            try
            {
                sale = await _sales.Add(SaleVm);

            }
            catch (Exception e)
            {
                if (e.Message != "Invalid Operation") throw;
                MessageType = "danger";
                Message = "Nothing to Create";
                return Page();
            }

            MessageType = "success";
            if (sale?.Id > 0)
            {
                Message = $"Sale: {sale.Id} - {sale.Customer.FullName} created!";
                return RedirectToPage("./Index");
            }
            if (true)
            {
                Message = $"Debt Payment received from {sale.Customer.FullName}!";
                return RedirectToPage("../DebtPayments/Index");
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                    Debug.WriteLine(e.ErrorMessage);
                return Page();
            }

            SaleDto sale;
            try
            {
                sale = await _sales.Save(SaleVm);

            }
            catch (Exception e)
            {
                if (e.Message == "Invalid Operation")
                {
                    MessageType = "danger";
                    Message = "Nothing to Create";
                    return Page();
                }
                else
                    throw;
            }

            MessageType = "success";
            Message = $"Sale: {sale.Id} - {sale.Customer.FullName} saved!";
            return RedirectToPage("./Saved");
        }
    }
}