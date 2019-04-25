using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Domain;

using static Domain.StockStatus;

namespace DTOs
{
    public class OutletDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        [Display(Name = "Outlet Name")]
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public Metadata Metadata { get; set; }

        public ICollection<ProductDto> Products { get; set; }
        public ICollection<ServiceDto> Services { get; set; }
        public ICollection<SaleDto> Sales { get; set; }

        #region Products Stock Status
        public int TotalProducts => Products?.Count() ?? 0;

        public IEnumerable<ProductDto> InStock =>
            Products?.Where(p => p.InventoryStockStatus > Empty);
        public int InStockCount => 
            InStock?.Count() ?? 0;

        public IEnumerable<ProductDto> InLimitedStock =>
            Products?.Where(p => p.InventoryStockStatus == LimitedStock);
        public int InLimitedStockCount
            => InLimitedStock?.Count() ?? 0;

        public IEnumerable<ProductDto> InLowStock =>
            Products?.Where(p => p.InventoryStockStatus == LowStock);
        public int InLowStockCount => 
            InLowStock?.Count() ?? 0;

        public IEnumerable<ProductDto> InEmptyStock =>
            Products?.Where(p => p.InventoryStockStatus == Empty);
        public int InEmptyStockCount => 
            InEmptyStock?.Count() ?? 0;

        public IEnumerable<ProductDto> InNegativeStock =>
            Products?.Where(p => p.InventoryStockStatus == Negative);

        public int InNegativeStockCount => 
            InNegativeStock?.Count() ?? 0;

        [DataType(DataType.Currency)]
        public decimal CostOfCurrentStock => 
            Products?.Where(p => p.InventoryStockStatus > Empty)
                .Sum(p => p.PricePurchase * p.InventoryTotalStock) ?? 0M;

        public IEnumerable<ProductDto> InEmptyOrNegativeStock =>
            Products?.Where(p => p.InventoryStockStatus <= Empty);

        public int InEmptyOrNegativeStockCount =>
            InEmptyOrNegativeStock?.Count() ?? 0;
        #endregion

        #region Sales Summary

        [Display(Name = "Sales Count")]
        public int SalesCount => Sales?.Count() ?? 0;

        [Display(Name = "Cost of Goods Sold")]
        [DataType(DataType.Currency)]
        public decimal CostOfGoodsSold => Sales?.SelectMany(s => s.Cart).Sum(s => s.NetPurchasePrice) ?? 0M;

        [Display(Name = "Sold Price of Goods")]
        [DataType(DataType.Currency)]
        public decimal PriceOfGoodsSold => Sales?.Sum(s => s.PaymentTotal) ?? 0M;

        [Display(Name = "Sales Profit")]
        [DataType(DataType.Currency)]
        public decimal SalesProfit => Sales?.Sum(s => s.Profit) ?? 0M;

        [Display(Name = "Sales Due")]
        [DataType(DataType.Currency)]
        public decimal SalesDue => Sales?.Sum(s => s.PaymentDue) ?? 0M;

        [Display(Name = "Profit Received (Profit - Due)",
            ShortName = "Received Profit")]
        [DataType(DataType.Currency)]
        public decimal SalesProfitReceived => SalesProfit - SalesDue;

        #endregion
    }
}
