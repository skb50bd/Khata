﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Brotal.Extensions;
using DTOs;
using Business.CRUD;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc.RazorPages;

using static Domain.StockStatus;

namespace WebUI.Pages.Reporting
{
    public class StockReportModel : PageModel
    {
        private IProductService _products;
        private IOutletService _outlets;
        private readonly PfService _pfService;

        public StockReportModel(
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
            Outlets = await _outlets.Get();
            Products = (await _products.Get(
                0,
                _pfService.CreateNewPf("", 1, int.MaxValue)))
                .OrderBy(p => p.Name);

            foreach(var o in Outlets)
            {
                o.Products = Products
                    .Where(p => p.OutletId == o.Id)
                    .ToList();
            }
        }
    }
}