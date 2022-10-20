namespace Domain;

public class CustomerInvoice : Invoice
{
    public int? SaleId { get; set; }
    public virtual Sale Sale { get; set; }
    public int? DebtPaymentId { get; set; }
    public virtual DebtPayment DebtPayment { get; set; }

    public int? OutletId { get; set; }
    public virtual Outlet Outlet { get; set; }

    public SaleType? Type { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}