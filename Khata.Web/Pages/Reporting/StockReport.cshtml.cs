using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc.RazorPages;

using static Khata.Domain.StockStatus;

namespace WebUI.Pages.Reporting
{
    public class StockReportModel : PageModel
    {
        private IProductService _products;
        private readonly PfService _pfService;

        public StockReportModel(
            IProductService products,
            PfService pfService)
        {
            _products = products;
            _pfService = pfService;
        }

        public string ForDate => DateTime.Today.ToString("dd MMM yyy");
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();

        public int TotalProducts => Products.Count();
        public int InStock
            => Products.Count(p => p.InventoryStockStatus > Empty);
        public int InLimitedStock
            => Products.Count(p => p.InventoryStockStatus == LimitedStock);
        public int InLowStock
            => Products.Count(p => p.InventoryStockStatus == LowStock);
        public int InEmptyStock
            => Products.Count(p => p.InventoryStockStatus == Empty);
        public int InNegativeStock
            => Products.Count(p => p.InventoryStockStatus == Negative);

        [DataType(DataType.Currency)]
        public decimal CostOfCurrentStock
            => Products.Where(p => p.InventoryStockStatus > Empty)
            .Sum(p => p.PricePurchase * p.InventoryTotalStock);

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Products = (await _products.Get(
                0,
                _pfService.CreateNewPf("", 1, int.MaxValue)))
                .OrderBy(p => p.Name);
        }
    }
}