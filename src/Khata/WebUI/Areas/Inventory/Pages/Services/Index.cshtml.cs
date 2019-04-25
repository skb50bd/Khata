using System.Collections.Generic;
using System.Threading.Tasks;

using Brotal;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Inventory.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly IOutletService _outlets;
        private readonly IServiceService _services;
        private readonly PfService _pfService;
        public IndexModel(
            PfService pfService, 
            IOutletService outlets,
            IServiceService services)
        {
            _pfService = pfService;
            _outlets = outlets;
            _services = services;
            Services = new PagedList<ServiceDto>();
        }

        public IEnumerable<OutletDto> Outlets { get; set; }
        public int CurrentOutletId { get; set; }
        public IPagedList<ServiceDto> Services { get; set; }
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
            int pageIndex = 1,
            int pageSize = 0)
        {
            Outlets = await _outlets.Get();
            outletId = outletId ?? 0;
            CurrentOutletId = (int)outletId;

            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Services = await _services.Get((int)outletId, Pf);
            return Page();
        }
    }
}

