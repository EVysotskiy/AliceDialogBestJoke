using System.Text.Json.Serialization;

namespace Domain.Model;

[Serializable]
public class JokeModel
{
    [JsonPropertyName("Text")]
    public string Text { get; set; }
}