using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SharedLibrary;

namespace WebUI.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly IServiceService _services;
        private readonly PfService _pfService;
        public IndexModel(PfService pfService, IServiceService services)
        {
            _pfService = pfService;
            _services = services;
            Services = new PagedList<ServiceDto>();
        }

        public IPagedList<ServiceDto> Services { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageIndex = 1,
            int pageSize = 0)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Services = await _services.Get(Pf);
            return Page();
        }
    }
}

