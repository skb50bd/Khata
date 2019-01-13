using System;
using System.ComponentModel.DataAnnotations;

namespace Khata.DTOs
{
    public class SupplierPaymentDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        public int SupplierId { get; set; }

        [Display(Name = "Supplier Name", ShortName = "Payee")]
        public string SupplierFullName { get; set; }

        [Display(Name = "Previous Payable")]
        [DataType(DataType.Currency)]
        public decimal PayableBefore { get; set; }

        [Display(Name = "Amount Paid", ShortName = "Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "New Payable")]
        [DataType(DataType.Currency)]
        public decimal PayableAfter { get; set; }

        public string Description { get; set; }

        [Display(Name = "Modifier")]
        public string MetadataModifier { get; set; }

        [Display(Name = "Last Modified")]
        public DateTimeOffset MetadataModificationTime { get; set; }
    }
}