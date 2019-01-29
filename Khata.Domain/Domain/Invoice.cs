using System;
using System.Collections.Generic;

namespace Khata.Domain
{
    public abstract class Invoice : TrackedDocument
    {
        public DateTimeOffset Date { get; set; }
        public virtual ICollection<InvoiceLineItem> Cart { get; set; } = new List<InvoiceLineItem>();
        public decimal PreviousDue { get; set; }
        public decimal PaymentSubtotal { get; set; }
        public decimal PaymentTotal => PreviousDue + PaymentSubtotal;
        public virtual decimal PaymentPayable => PaymentTotal;
        public decimal PaymentPaid { get; set; }
        public decimal DueAfter => PaymentPayable - PaymentPaid;

        public string DateLocalDate => Date.ToString("dd MMM yyyy");
    }
}