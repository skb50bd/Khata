using System;
using System.Collections.Generic;

using Khata.Domain;

namespace Khata.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public bool IsRemoved { get; set; }
        public SaleType SaleType { get; set; }
        public CustomerDto Customer { get; set; }
        public ICollection<LineItemDto> Cart { get; set; }
        public decimal PaymentSubTotal { get; set; }
        public decimal PaymentDiscountCash { get; set; }
        public float PaymentDiscountPercentage { get; set; }
        public decimal PaymentTotal { get; set; }
        public decimal PaymentPaid { get; set; }
        public decimal PaymentDue { get; set; }

        public string MetadataModifier { get; set; }
        public DateTimeOffset MetadataModificationTime { get; set; }
    }
}