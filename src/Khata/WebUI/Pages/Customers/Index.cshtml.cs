using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;
using Business.Abstractions;

namespace WebUI.Pages.Customers
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customers;
        private readonly PfService _pfService;
        public IndexModel(ICustomerService customers, PfService pfService)
        {
            _customers = customers;
            _pfService = pfService;
            Customers = new PagedList<CustomerDto>();
        }

        public IPagedList<CustomerDto> Customers { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Customers = await _customers.Get(Pf);
            return Page();
        }
    }
}
