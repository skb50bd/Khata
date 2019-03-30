using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Business.Abstractions;
using Business.PageFilterSort;

using Domain;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace WebUI.Pages.Reporting
{
    public class DueReportModel : PageModel
    {
        private readonly ICustomerService _customers;
        private readonly PfService _pfService;

        public DueReportModel(
            ICustomerService customers,
            PfService pfService)
        {
            _customers = customers;
            _pfService = pfService;
        }


        public IEnumerable<CustomerDto> Customers;
        public int Count => Customers?.Count() ?? 0;
        [DataType(DataType.Currency)]
        public decimal TotalDue => Customers?.Sum(c => c.Debt) ?? 0M;
        [DataType(DataType.Currency)]
        public decimal AverageDue => Count == 0 ? 0M : TotalDue / Count;

        public string ForDate => Clock.Today.ToString("dd MMM yyy");

        public async Task<IActionResult> OnGetAsync()
        {
            Customers = (await _customers.Get(
                _pfService.CreateNewPf("", 1, int.MaxValue)))
                .Where(c => c.Debt > 0)
                .OrderByDescending(c => c.Debt);
            return Page();
        }
    }
}