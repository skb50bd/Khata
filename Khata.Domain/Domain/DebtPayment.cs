namespace Khata.Domain
{
    public class DebtPayment : TrackedEntity
    {
        public virtual Customer Customer { get; set; }
        public decimal PreviousDebt { get; set; }
        public decimal Amount { get; set; }
        public decimal NewDebt => PreviousDebt - Amount;
        public Metadata Metadata { get; set; }
    }
}