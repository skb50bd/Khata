using System;

using Brotal.Extensions;

using Newtonsoft.Json;

namespace Domain;

public class Metadata : Entity
{
    public string Creator { get; set; }
    public DateTimeOffset CreationTime { get; set; }

    public string Modifier { get; set; }
    public DateTimeOffset ModificationTime { get; set; }

    [JsonIgnore]
    public string Summary => "Updated " + ModificationTime.Natural()
                                        + " ago by " + Modifier;
    [JsonIgnore]
    public string ModifiedAt => ModificationTime.ToString("dd/MM/yyyy HH:mm");

    [JsonIgnore]
    public string CreatedAt => CreationTime.ToString("dd/MM/yyyy HH:mm");

    [JsonIgnore]
    public string CreatedAgo => CreationTime.Natural();

    [JsonIgnore]
    public string UpdatedAgo => ModificationTime.Natural();

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

    //private Metadata(string creator, DateTimeOffset creationTime, string modifier)
    //{
    //    Creator = creator;
    //    CreationTime = creationTime;
    //    Modifier = modifier;
    //}

    public static Metadata CreatedNew(string username) => new Metadata(username);

    public Metadata Modified(string username)
    {
        Modifier = username;
        ModificationTime = DateTimeOffset.Now;

        return this;
    }
}