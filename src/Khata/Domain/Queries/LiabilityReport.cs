using System.ComponentModel.DataAnnotations;

namespace Queries
{
    public class LiabilityReport : Report
    {
        [Display(Name = "Suppliers with Due")]
        public int DueCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalDue { get; set; }

        [Display(Name = "Unpaid Employees")]
        public int UnpaidEmployees { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnpaidAmount { get; set; }
    }
}
