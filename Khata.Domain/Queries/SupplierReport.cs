using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public class SupplierReport : IndividaulReport
    {
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Payable { get; set; }

        public int PurchasesCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal PurchasesWorth { get; set; }

        [DataType(DataType.Currency)]
        public decimal PurchasePaid { get; set; }

        public int SupplierPaymentsCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SupplierPaymentPaid { get; set; }

        public int PurchaseReturnCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal PurchaseReturnAmount { get; set; }
    }
}
