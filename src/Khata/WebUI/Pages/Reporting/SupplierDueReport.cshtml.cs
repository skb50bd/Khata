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
    public class SupplierDueReportModel : PageModel
    {
        private readonly ISupplierService _suppliers;
        private readonly PfService _pfService;

        public SupplierDueReportModel(
            ISupplierService suppliers,
            PfService pfService)
        {
            _suppliers = suppliers;
            _pfService = pfService;
        }


        public IEnumerable<SupplierDto> Suppliers;
        public int Count => Suppliers?.Count() ?? 0;
        [DataType(DataType.Currency)]
        public decimal TotalDue => Suppliers?.Sum(c => c.Payable) ?? 0M;
        [DataType(DataType.Currency)]
        public decimal AverageDue => Count == 0 ? 0M : TotalDue / Count;

        public string ForDate => Clock.Today.ToString("dd MMM yyy");

        public async Task<IActionResult> OnGetAsync()
        {
            Suppliers = (await _suppliers.Get(
                 _pfService.CreateNewPf("", 1, int.MaxValue)))
                .Where(c => c.Payable > 0)
                .OrderByDescending(c => c.Payable);
            return Page();
        }
    }
}