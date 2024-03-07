namespace Domain.Model
{
    public interface ITimeStampedModel
    {
        DateTime CreatedAt { get; set; }
        DateTime LastModified { get; set; }
    }
}