using System;

namespace Khata.Domain
{
    public class Metadata : Entity
    {
        public string Creator { get; private set; }
        public DateTimeOffset CreationTime { get; private set; }

        public string Modifier { get; private set; }
        public DateTimeOffset ModificationTime { get; private set; }

        public string ModifiedAtLocalTime
        {
            get
            {
                if (ModificationTime.Date == DateTime.Today)
                    return "Today at " + ModificationTime.ToString("HH:mm");
                else if (ModificationTime.Date.AddDays(1) == DateTime.Today)
                    return "Yesterday at " + ModificationTime.ToString("HH:mm");
                else
                    return ModificationTime.ToString("dd/MM/yyyy");
            }
        }

        public Metadata()
        {

        }

        private Metadata(string username)
        {
            Creator = username;
            Modifier = username;
            CreationTime = DateTimeOffset.Now;
            ModificationTime = DateTimeOffset.Now;
        }

        private Metadata(string creator, DateTimeOffset creationTime, string modifier)
        {
            Creator = creator;
            CreationTime = creationTime;
            Modifier = modifier;
        }

        public static Metadata CreatedNew(string username) => new Metadata(username);

        public Metadata Modified(string username)
        {
            Modifier = username;
            ModificationTime = DateTimeOffset.Now;

            return this;
        }
    }
}