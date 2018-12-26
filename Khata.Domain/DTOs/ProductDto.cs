using System;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public decimal Stock { get; set; }

        public decimal Warehouse { get; set; }

        public decimal AlertAt { get; set; }

        [Display(Name = "Total Stock")]
        public decimal TotalStock { get; set; }

        public StockStatus Status { get; set; }

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

        [Display(Name = "Last Modified By", ShortName = "Modifier")]
        public string Modifier { get; set; }

        [Display(Name = "Last Modified At", ShortName = "Last Modified By")]
        public DateTimeOffset Modified { get; set; }
    }
}