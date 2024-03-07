using System.Text.Json.Serialization;

namespace Domain.Model;

[Serializable]
public class UserState
{
    [JsonPropertyName("jokeIdListen")] 
    private List<long> _jokeIdListen { get; set; } = new();

    public long[] GetJokeIdIdListened()
    {
        if (_jokeIdListen == null)
        {
            _jokeIdListen = new List<long>();
        }

        return _jokeIdListen.ToArray();
    }

    public void AddListenedJoke(long jokeId)
    {
        _jokeIdListen.Add(jokeId);
    }
    
}