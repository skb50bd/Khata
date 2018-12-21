using System;

namespace Khata.Domain
{
    public class Metadata: Entity
    {
        public string Creator { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public string Modifier { get; set; }
        public DateTimeOffset ModificationTime { get; set; }
    }
}