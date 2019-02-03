
using System.ComponentModel.DataAnnotations;

using Khata.Domain;

namespace Khata.DTOs
{
    public class CustomerInvoiceDto : InvoiceDto
    {
        public int? SaleId { get; set; }
        public virtual SaleDto Sale { get; set; }
        public int? DebtPaymentId { get; set; }
        public virtual DebtPaymentDto DebtPayment { get; set; }

        public SaleType? Type { get; set; }
        public int CustomerId { get; set; }
        public virtual CustomerDto Customer { get; set; }

        [DataType(DataType.Currency)]
        public decimal PaymentDiscountCash { get; set; }

        public decimal PaymentDiscountPercentage =>
            PaymentSubtotal != 0
            ? PaymentDiscountCash / PaymentSubtotal * 100
            : 0;

        [DataType(DataType.Currency)]
        public override decimal PaymentPayable => PaymentTotal - PaymentDiscountCash;
    }
}