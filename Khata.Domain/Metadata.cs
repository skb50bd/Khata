using System;

namespace Khata.Domain
{
    public class Metadata
    {
        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public string ModifierId { get; set; }
        public virtual ApplicationUser Modifier { get; set; }
        public DateTimeOffset ModificationTime { get; set; }
    }
}