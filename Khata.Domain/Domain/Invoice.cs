using System;
using System.Collections.Generic;

namespace Khata.Domain
{
    public abstract class Invoice : TrackedDocument
    {
        public DateTimeOffset Date { get; set; }
        public virtual ICollection<InvoiceLineItem> Cart { get; set; } 
            = new List<InvoiceLineItem>();

        public decimal PreviousDue { get; set; }
        public decimal PaymentSubtotal { get; set; }
        public decimal PaymentTotal => PreviousDue + PaymentSubtotal;
        public decimal PaymentPayable => PaymentTotal - PaymentDiscountCash;
        public decimal PaymentPaid { get; set; }
        public decimal DueAfter => PaymentPayable - PaymentPaid;

        public decimal PaymentDiscountCash { get; set; }
        public decimal PaymentDiscountPercentage => PaymentSubtotal > 0
            ? PaymentDiscountCash / PaymentSubtotal * 100
            : 0M;

        public string DateLocalDate => Date.ToString("dd MMM yyyy");
    }
}