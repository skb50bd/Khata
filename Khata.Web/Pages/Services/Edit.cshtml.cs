using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using SharedLibrary;

namespace WebUI.Pages.Services
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
        public ServiceViewModel ServiceVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _db.Services.GetById((int)id);

            if (service == null)
            {
                return NotFound();
            }

            ServiceVm = _mapper.Map<ServiceViewModel>(service);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newService = _mapper.Map<Service>(ServiceVm);
            var originalService = await _db.Services.GetById(newService.Id);
            var meta = originalService.Metadata.Modified(User.Identity.Name);
            originalService.SetValuesFrom(newService);
            originalService.Metadata = meta;

            try
            {
                await _db.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExists(newService.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Service: {newService.Id} - {newService.Name} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> ProductExists(int id)
        {
            return await _db.Products.Exists(id);
        }
    }
}
