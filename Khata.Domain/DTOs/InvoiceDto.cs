using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public bool IsRemoved { get; set; }

        public DateTimeOffset Date { get; set; }

        public virtual ICollection<InvoiceLineItemDto> Cart { get; set; } = new List<InvoiceLineItemDto>();

        [DataType(DataType.Currency)]
        public decimal PreviousDue { get; set; }

        [DataType(DataType.Currency)]
        public decimal PaymentSubtotal { get; set; }

        [DataType(DataType.Currency)]
        public decimal PaymentTotal => PreviousDue + PaymentSubtotal;

        [DataType(DataType.Currency)]
        public virtual decimal PaymentPayable => PaymentTotal;

        [DataType(DataType.Currency)]
        public decimal PaymentPaid { get; set; }

        [DataType(DataType.Currency)]
        public decimal DueAfter => PaymentPayable - PaymentPaid;

        public string DateLocalDate => Date.ToString("dd MMM yyyy");

        public Metadata Metadata { get; set; }
    }
}