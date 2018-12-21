using System.Collections.Generic;

namespace Khata.Domain
{
    public class Product : TrackedEntity
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public virtual IList<Category> Categories { get; set; } = new List<Category>();
        public Metadata Metadata { get; set; }
    }
}