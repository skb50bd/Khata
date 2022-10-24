using System.Text.Json.Serialization;
using Humanizer;
using JsonNetIgnore = Newtonsoft.Json.JsonIgnoreAttribute;

namespace Domain;

public class Metadata : Entity
{
    public required string Creator { get; init; }
    public required DateTimeOffset CreationTime { get; init; }

    public required string Modifier { get; set; }
    public required DateTimeOffset ModificationTime { get; set; }

    [JsonNetIgnore]
    [JsonIgnore]
    public string Summary => $"Updated {ModificationTime.Humanize()} by {Modifier}";
    
    [JsonNetIgnore]
    [JsonIgnore]
    public string ModifiedAt => ModificationTime.ToString("dd/MM/yyyy HH:mm");

    [JsonNetIgnore]
    [JsonIgnore]
    public string CreatedAt => CreationTime.ToString("dd/MM/yyyy HH:mm");

    [JsonNetIgnore]
    [JsonIgnore]
    public string CreatedAgo => CreationTime.Humanize();

    [JsonNetIgnore]
    [JsonIgnore]
    public string UpdatedAgo => ModificationTime.Humanize();

    public Metadata()
    {
    }
    
    public static Metadata CreatedNew(string username) => 
        new()
        {
            Creator          = username,
            Modifier         = username,
            CreationTime     = new UtcDateTimeProvider().Now,
            ModificationTime = new UtcDateTimeProvider().Now
        };

    public Metadata ModifiedBy(string username)
    {
        Modifier         = username;
        ModificationTime = new UtcDateTimeProvider().Now;

        return this;
    }
}