using Khata.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static Khata.Domain.StockStatus;

namespace Khata.DTOs
{
    public class OutletDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        [Display(Name = "Outlet Name")]
        public string Title { get; set; }
        public string Slogan { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Metadata Metadata { get; set; }

        public ICollection<ProductDto> Products { get; set; }
        public ICollection<SaleDto> Sales { get; set; }

        #region Products Stock Status
        public int TotalProducts => Products?.Count() ?? 0;
        public int InStock
            => Products?.Count(p => p.InventoryStockStatus > Empty) ?? 0;
        public int InLimitedStock
            => Products?.Count(p => p.InventoryStockStatus == LimitedStock) ?? 0;
        public int InLowStock
            => Products?.Count(p => p.InventoryStockStatus == LowStock) ?? 0;
        public int InEmptyStock
            => Products?.Count(p => p.InventoryStockStatus == Empty) ?? 0;
        public int InNegativeStock
            => Products?.Count(p => p.InventoryStockStatus == Negative) ?? 0;

        [DataType(DataType.Currency)]
        public decimal CostOfCurrentStock
            => Products?.Where(p => p.InventoryStockStatus > Empty)
            .Sum(p => p.PricePurchase * p.InventoryTotalStock) ?? 0M; 
        #endregion
    }
}
