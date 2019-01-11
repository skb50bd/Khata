using System.Threading.Tasks;

using AutoMapper;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.Customers
{
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customers;
        private readonly IMapper _mapper;

        public EditModel(ICustomerService customers, IMapper mapper)
        {
            _customers = customers;
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

            var customer = await _customers.Get((int)id);

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
            CustomerDto customer;
            try
            {
                customer = await _customers.Update(CustomerVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists((int)CustomerVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Customer: {customer.Id} - {customer.FullName} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _customers.Exists(id);
        }
    }
}
