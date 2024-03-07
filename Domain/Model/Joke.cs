namespace Domain.Model;

public class Joke : ITimeStampedModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Text { get; set; }
    public bool IsBlackList { get; set; }
    public long CountLiked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public Joke(long userId, string text)
    {
        UserId = userId;
        Text = text;
        IsBlackList = false;
        CountLiked = 0;
    }
}