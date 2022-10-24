namespace Domain;

public class Customer : Person
{
    public string? CompanyName { get; set; }
    public decimal Debt { get; set; }

    public virtual ICollection<Sale> Purchases { get; set; } = new List<Sale>();
    public virtual ICollection<DebtPayment> DebtPayments { get; set; } = new List<DebtPayment>();
    public virtual ICollection<Refund> Refunds { get; set; } = new List<Refund>();
}