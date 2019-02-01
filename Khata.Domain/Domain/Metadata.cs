using System;

using DateTimeExtensions;
namespace Khata.Domain
{
    public class Metadata : Entity
    {
        public string Creator { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public string Modifier { get; set; }
        public DateTimeOffset ModificationTime { get; set; }


        public string Summary => "Updated " + ModificationTime.DateTime.ToNaturalText(DateTime.Now)
                                    + " ago by " + Modifier;
        public string ModifiedAt => ModificationTime.ToString("dd/MM/yyyy HH:mm");
        public string CreatedAt => CreationTime.ToString("dd/MM/yyyy HH:mm");

        public string CreatedAgo => CreationTime.DateTime.ToNaturalText(DateTime.Now);
        public string UpdatedAgo => ModificationTime.DateTime.ToNaturalText(DateTime.Now);

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