using System.ComponentModel.DataAnnotations;

namespace Domain.Reports
{
    public class Liability : Report
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
