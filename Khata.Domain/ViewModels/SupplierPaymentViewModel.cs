using System.ComponentModel.DataAnnotations;

namespace Khata.ViewModels
{
    public class SupplierPaymentViewModel
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        [Display(Name = "Amount Paid", ShortName = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Previous Payable")]
        [DataType(DataType.Currency)]
        public decimal PayableBefore { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}