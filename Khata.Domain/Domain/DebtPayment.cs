namespace Khata.Domain
{
    public class DebtPayment : TrackedEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public decimal DebtBefore { get; set; }
        public decimal Amount { get; set; }
        public decimal DebtAfter => DebtBefore - Amount;
        public string Description { get; set; }
    }
}