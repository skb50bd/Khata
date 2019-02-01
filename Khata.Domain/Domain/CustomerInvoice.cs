namespace Khata.Domain
{
    public class CustomerInvoice : Invoice
    {
        public int? SaleId { get; set; }
        public virtual Sale Sale { get; set; }
        public int? DebtPaymentId { get; set; }
        public virtual DebtPayment DebtPayment { get; set; }

        public SaleType? Type { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public decimal PaymentDiscountCash { get; set; }
        public decimal PaymentDiscountPercentage => PaymentSubtotal > 0
            ? PaymentDiscountCash / PaymentSubtotal * 100
            : 0M;
        public override decimal PaymentPayable => PaymentTotal - PaymentDiscountCash;
    }
}