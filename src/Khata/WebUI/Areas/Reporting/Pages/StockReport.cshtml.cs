using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Business.Abstractions;
using Business.PageFilterSort;
using DTOs;

using Microsoft.AspNetCore.Mvc.RazorPages;

using static Domain.StockStatus;

namespace WebUI.Areas.Reporting.Pages;

public class StockReportModel : PageModel
{
    private readonly IProductService _products;
    private readonly IOutletService _outlets;
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

    public string ForDate => Clock.Now.LocalDateTime();
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
    public IEnumerable<OutletDto> Outlets { get; set; } = new List<OutletDto>();

    public int TotalProducts => Products.Count();
    //public int InStockCount
    //    => Products.Count(p => p.InventoryStockStatus > Empty);
    //public int InLimitedStockCount
    //    => Products.Count(p => p.InventoryStockStatus == LimitedStock);
    //public int InLowStockCount
    //    => Products.Count(p => p.InventoryStockStatus == InLowStock);
    //public int InEmptyStockCount
    //    => Products.Count(p => p.InventoryStockStatus == Empty);
    //public int InNegativeStockCount
    //=> Products.Count(p => p.InventoryStockStatus == Negative);

    public IEnumerable<ProductDto> InStock =>
        Products.Where(p => p.InventoryStockStatus > Empty);

    public int InStockCount => InStock.Count();

    public IEnumerable<ProductDto> InLowStock =>
        Products.Where(p => p.InventoryStockStatus == LowStock);

    public int LowStockCount => InLowStock.Count();

    public IEnumerable<ProductDto> InEmptyOrNegativeStock =>
        Products.Where(p => p.InventoryStockStatus <= Empty);

    public int EmptyOrNegativeStockCount => InEmptyOrNegativeStock.Count();

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

        foreach (var o in Outlets)
        {
            o.Products = Products
                .Where(p => p.OutletId == o.Id)
                .ToList();
        }
    }
}