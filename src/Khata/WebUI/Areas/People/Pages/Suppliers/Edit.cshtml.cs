using System.Threading.Tasks;

using AutoMapper;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using ViewModels;

namespace WebUI.Areas.People.Pages.Suppliers
{
    public class EditModel : PageModel
    {
        private readonly ISupplierService _suppliers;
        private readonly IMapper _mapper;

        public EditModel(ISupplierService suppliers, IMapper mapper)
        {
            _suppliers = suppliers;
            _mapper = mapper;
        }

        [BindProperty]
        public SupplierViewModel SupplierVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _suppliers.Get((int)id);

            if (supplier == null)
            {
                return NotFound();
            }

            SupplierVm = _mapper.Map<SupplierViewModel>(supplier);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            SupplierDto supplier;
            try
            {
                supplier = await _suppliers.Update(SupplierVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SupplierExists((int)SupplierVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Supplier: {supplier.Id} - {supplier.FullName} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> SupplierExists(int id)
        {
            return await _suppliers.Exists(id);
        }
    }
}
