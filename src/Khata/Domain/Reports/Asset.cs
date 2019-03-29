using System.ComponentModel.DataAnnotations;

namespace Domain.Reports
{
    public class Asset : Report
    {
        [Display(Name = "Customers with Due")]
        public int DueCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalDue { get; set; }

        [Display(Name = "Cash in Hand")]
        [DataType(DataType.Currency)]
        public decimal Cash { get; set; }

        [Display(Name = "Products in Stock")]
        public int InventoryCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal InventoryWorth { get; set; }
    }
}
