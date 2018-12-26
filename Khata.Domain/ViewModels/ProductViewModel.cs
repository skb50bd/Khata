using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Name must be at least 5 characters long")]
        [MaxLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Unit { get; set; } = "Piece";

        [Required]
        [Display(Name = "Retail Price")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PriceRetail { get; set; }

        [Display(Name = "Whole Sale Price")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PriceBulk { get; set; }

        [Display(Name = "Purchase Price")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PricePurchase { get; set; } = 0;

        [Display(Name = "Minimum Price")]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal PriceMargin { get; set; } = 0;

        [Required]
        [Display(Name = "Stock")]
        public decimal InventoryStock { get; set; } = 0;

        [Display(Name = "Warehouse")]
        public decimal InventoryWarehouse { get; set; } = 0;

        [Display(Name = "Minimum Total Stock Required")]
        public decimal InventoryAlertAt { get; set; } = 0;

        [Display(Name = "Status")]
        public StockStatus InventoryStockStatus { get; private set; }
    }
}