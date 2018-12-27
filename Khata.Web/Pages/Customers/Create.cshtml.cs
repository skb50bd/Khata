using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public CreateModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            CustomerVm = new CustomerViewModel();
            return Page();
        }

        [BindProperty]
        public CustomerViewModel CustomerVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var customer = _mapper.Map<Customer>(CustomerVm);
            customer.Metadata = Metadata.CreatedNew(User.Identity.Name);
            _db.Customers.Add(customer);
            await _db.CompleteAsync();

            Message = $"Customer: {customer.Id} - {customer.FullName} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}