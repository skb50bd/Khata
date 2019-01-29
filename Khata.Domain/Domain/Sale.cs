using System;
using System.Collections.Generic;
using System.Linq;

namespace Khata.Domain
{
    public class Sale : TrackedDocument, IDeposit
    {
        public int InvoiceId { get; set; }
        public virtual CustomerInvoice Invoice { get; set; }

        public DateTimeOffset SaleDate { get; set; }
        public SaleType Type { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleLineItem> Cart { get; set; }
        public PaymentInfo Payment { get; set; }

        public decimal Profit => Cart?.Sum(li => li.Profit) ?? 0;

        public string Description { get; set; }

        public decimal Amount => Payment.Paid;
        public string TableName => nameof(Sale);
        public int? RowId => Id;
    }
}