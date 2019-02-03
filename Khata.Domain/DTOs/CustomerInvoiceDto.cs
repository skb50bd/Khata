
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
    }
}