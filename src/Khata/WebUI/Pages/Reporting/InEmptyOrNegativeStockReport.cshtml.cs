using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Business.CRUD;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Mvc.RazorPages;

using static Domain.StockStatus;

namespace WebUI.Pages.Reporting
{
    public class InEmptyOrNegativeStockReport : PageModel
    {
        private readonly IProductService _products;
        private readonly IOutletService _outlets;
        private readonly PfService _pfService;

        public InEmptyOrNegativeStockReport(
            IProductService products,
            IOutletService outlets,
            PfService pfService)
        {
            _products = products;
            _outlets = outlets;
            _pfService = pfService;

        }

        public string ForDate => DateTime.Now.LocalDateTime();
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
        public IEnumerable<OutletDto> Outlets { get; set; } = new List<OutletDto>();

        public IEnumerable<ProductDto> InEmptyOrNegativeStock =>
            Products.Where(p => p.InventoryStockStatus <= Empty);

        public int InEmptyOrNegativeStockCount => InEmptyOrNegativeStock.Count();

        public async Task OnGetAsync()
        {
            Outlets = await _outlets.Get();
            Products = (await _products.Get(
                0,
                _pfService.CreateNewPf("", 1, int.MaxValue)))
                .OrderBy(p => p.Name);

            foreach (var o in Outlets)
            {
                o.Products = InEmptyOrNegativeStock
                    .Where(p => p.OutletId == o.Id)
                    .ToList();
            }
        }
    }
}