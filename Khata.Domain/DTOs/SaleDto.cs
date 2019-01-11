using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }

        [Display(Name = "Type")]
        public SaleType SaleType { get; set; }

        public CustomerDto Customer { get; set; }

        public ICollection<LineItemDto> Cart { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal PaymentSubTotal { get; set; }

        [Display(Name = "Discount")]
        [DataType(DataType.Currency)]
        public decimal PaymentDiscountCash { get; set; }

        [Display(Name = "Discount (%)")]
        public float PaymentDiscountPercentage { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal PaymentTotal { get; set; }

        [Display(Name = "Paid")]
        [DataType(DataType.Currency)]
        public decimal PaymentPaid { get; set; }

        [Display(Name = "Due")]
        [DataType(DataType.Currency)]
        public decimal PaymentDue { get; set; }

        [Display(Name = "Modified by")]
        public string MetadataModifier { get; set; }

        [Display(Name = "Modified at")]
        public DateTimeOffset MetadataModificationTime { get; set; }
    }
}