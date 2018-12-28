namespace Khata.Domain
{
    public class TrackedEntity : Entity
    {
        public bool IsRemoved { get; set; }
        public Metadata Metadata { get; set; }
    }
}
