using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using StonedExtensions;

namespace WebUI.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public EditModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [BindProperty]
        public CustomerViewModel CustomerVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _db.Customers.GetById((int)id);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerVm = _mapper.Map<CustomerViewModel>(customer);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newCustomer = _mapper.Map<Customer>(CustomerVm);
            var originalCustomer = await _db.Customers.GetById(newCustomer.Id);
            var meta = originalCustomer.Metadata.Modified(User.Identity.Name);
            originalCustomer.SetValuesFrom(newCustomer);
            originalCustomer.Metadata = meta;

            try
            {
                await _db.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(newCustomer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Customer: {newCustomer.Id} - {newCustomer.FullName} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _db.Customers.Exists(id);
        }
    }
}
