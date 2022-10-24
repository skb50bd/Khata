﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstractions;
using Business.PageFilterSort;
using DTOs;

using Microsoft.AspNetCore.Mvc.RazorPages;

using static Domain.StockStatus;

namespace WebUI.Areas.Reporting.Pages;

public class InLowStockReport : PageModel
{
    private readonly IProductService _products;
    private readonly IOutletService _outlets;
    private readonly PfService _pfService;

    public InLowStockReport(
        IProductService products,
        IOutletService outlets,
        PfService pfService)
    {
        _products = products;
        _outlets = outlets;
        _pfService = pfService;

    }

    public string ForDate => Clock.Now.LocalDateTime();
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
    public IEnumerable<OutletDto> Outlets { get; set; } = new List<OutletDto>();

    public int TotalProducts => Products.Count();

    public IEnumerable<ProductDto> InLowStock =>
        Products.Where(p => p.InventoryStockStatus == LowStock);

    public int LowStockCount => InLowStock.Count();

    public async Task OnGetAsync()
    {
        Outlets = await _outlets.Get();
        Products = (await _products.Get(
                0,
                _pfService.CreateNewPf("", 1, int.MaxValue)))
            .OrderBy(p => p.Name);

        foreach (var o in Outlets)
        {
            o.Products = InLowStock
                .Where(p => p.OutletId == o.Id)
                .ToList();
        }
    }
}