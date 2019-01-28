using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class SalaryPaymentDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name", ShortName = "Issuee")]
        public string EmployeeFullName { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Balance Before")]
        public decimal BalanceBefore { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Balance After")]
        public decimal BalanceAfter { get; set; }

        public string Description { get; set; }

        public Metadata Metadata { get; set; }
    }
}