using Domain.Model;

namespace Domain.Services;

public interface IJokeService
{
    Task<Joke?> Get(long id);

    Task<Joke?> GetRandomByExcludeUserId(IReadOnlyList<long> excludeUserId, IReadOnlyList<long> excludeJokeId, bool isBlackList = false);
    Task<Joke?> GetRandomByExcludeUserId(IReadOnlyList<long> excludeUserId, bool isBlackList = false);
    Task<Joke[]?> GetByUserId(long senderUserId, bool isRead = false);
    Task<Joke> Add(Joke newJoke);
    Task<Joke> Update(Joke joke);
    Task<Joke?> Like(long jokeId);
}