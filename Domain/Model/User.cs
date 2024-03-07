using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Domain.Model;

public class User : ITimeStampedModel
{
    public long Id { get; set; }
    public string PlatformId { get; set; }
    public string LastCommandId { get; set; }
    [Column(TypeName = "jsonb")]
    public string State { get; set; }
    public long LastSoundJokeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public User(string platformId)
    {
        PlatformId = platformId;
        LastCommandId = string.Empty;
        State = JsonSerializer.Serialize(new UserState());
    }
}