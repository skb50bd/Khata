using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class SalaryPaymentViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Amount Paid", ShortName = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public string Description { get; set; }
    }
}