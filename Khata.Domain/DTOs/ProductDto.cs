using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public bool IsRemoved { get; set; }

        public string Name { get; set; }

        public int OutletId { get; set; }
        public OutletDto Outlet { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public decimal InventoryStock { get; set; }

        public decimal InventoryWarehouse { get; set; }

        public decimal InventoryAlertAt { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPurchaseCost => InventoryTotalStock * PricePurchase;

        [Display(Name = "Total Stock")]
        public decimal InventoryTotalStock { get; set; }

        public StockStatus InventoryStockStatus { get; set; }

        [Display(Name = "Retail Price")]
        [DataType(DataType.Currency)]
        public decimal PriceRetail { get; set; }

        [Display(Name = "Whole Sale Price", ShortName = "Bulk")]
        [DataType(DataType.Currency)]
        public decimal PriceBulk { get; set; }

        [Display(Name = "Marginal Price", ShortName = "Margin")]
        [DataType(DataType.Currency)]
        public decimal PriceMargin { get; set; }

        [Display(Name = "Purchase Price", ShortName = "Purchase")]
        [DataType(DataType.Currency)]
        public decimal PricePurchase { get; set; }

        public Metadata Metadata { get; set; }
    }
}