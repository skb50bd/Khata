using System;
using System.Collections.Generic;

namespace Khata.Domain
{
    public class Invoice : TrackedEntity
    {
        public int? SaleId { get; set; }
        public virtual Sale Sale { get; set; }
        public int? DebtPaymentId { get; set; }
        public virtual DebtPayment DebtPayment { get; set; }

        public DateTimeOffset Date { get; set; }
        public SaleType? Type { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<InvoiceLineItem> Cart { get; set; } = new List<InvoiceLineItem>();
        public decimal PreviousDue { get; set; }
        public decimal PaymentSubtotal { get; set; }
        public decimal PaymentDiscountCash { get; set; }
        public decimal PaymentDiscountPercentage { get; set; }
        public decimal PaymentTotal => PreviousDue + PaymentSubtotal;
        public decimal PaymentPayable => PaymentTotal - PaymentDiscountCash;
        public decimal PaymentPaid { get; set; }
        public decimal DueAfter => PaymentPayable - PaymentPaid;

        public string DateLocalDate => Date.ToString("dd/MM/yyyy");
    }
}