using System;

namespace Domain
{
    public class DebtPayment : TrackedDocument, IDeposit
    {
        public DateTimeOffset PaymentDate { get; set; }
        public int InvoiceId { get; set; }
        public virtual CustomerInvoice Invoice { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public decimal DebtBefore { get; set; }
        public decimal Amount { get; set; }
        public decimal DebtAfter => DebtBefore - Amount;
        public string Description { get; set; }

        public string TableName => nameof(DebtPayment);
        public int? RowId => Id;
    }
}