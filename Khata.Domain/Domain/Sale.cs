namespace Khata.Domain
{
    public class Sale : TrackedEntity
    {
        public virtual Customer Customer { get; set; }
    }
}