namespace Khata.Domain
{
    public class DebtPayment : TrackedEntity
    {
        public virtual Customer Customer { get; set; }
    }
}