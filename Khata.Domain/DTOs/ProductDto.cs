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

        public decimal Godown { get; set; }

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

        [Display(Name = "Margin")]
        [DataType(DataType.Currency)]
        public decimal PriceMargin { get; set; }
        public string Modifier { get; set; }
    }
}