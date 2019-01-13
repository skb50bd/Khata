using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Suppliers
{
    public class DetailsModel : PageModel
    {
        private readonly ISupplierService _suppliers;
        public DetailsModel(ISupplierService suppliers)
        {
            _suppliers = suppliers;
        }

        public SupplierDto Supplier { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Supplier = await _suppliers.Get((int)id);

            if (Supplier is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
