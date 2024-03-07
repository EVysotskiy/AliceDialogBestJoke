using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services;

public class JokeService : IJokeService
{
    private readonly JokeRepository _jokeRepository;
    private readonly ILogger _logger;

    public JokeService(JokeRepository jokeRepository, ILogger<JokeService> logger)
    {
        _jokeRepository = jokeRepository;
        _logger = logger;
    }
    
    public async Task<Joke?> Get(long id)
    {
        return await _jokeRepository.First(x => x.Id == id);
    }
    
    public async Task<Joke?> GetRandomByExcludeUserId(IReadOnlyList<long> excludeUserId, IReadOnlyList<long> excludeJokeId, bool isBlackList = false)
    {
        var jokes = await _jokeRepository.Where(x =>
                excludeUserId.Contains(x.UserId) == false
                && excludeJokeId.Contains(x.Id) == false &&
                x.IsBlackList == isBlackList,
            x => x.CountLiked, 20);
        
        if (jokes == null || jokes.Length <= 0)
        {
            return null;
        }

        var random = new Random();
        var countJokes = jokes.Length;
        var randomIndex = random.Next(0, countJokes);

        var randomJoke = jokes[randomIndex];
        return randomJoke;
    }
    
    public async Task<Joke?> GetRandomByExcludeUserId(IReadOnlyList<long> excludeUserId,bool isBlackList = false)
    {
        var jokes = await _jokeRepository.Where(x => excludeUserId.Contains(x.UserId) == false && x.IsBlackList == isBlackList,x => x.CountLiked);
        if (jokes == null || jokes.Length <= 0)
        {
            return null;
        }

        var random = new Random();
        var countJokes = jokes.Length;
        var randomIndex = random.Next(0, countJokes);

        var randomJoke = jokes[randomIndex];
        return randomJoke;
    }
    
    public async Task<Joke[]?> GetByUserId(long senderUserId, bool isBlackList = false)
    {
        return await _jokeRepository.Where(x => x.UserId == senderUserId && x.IsBlackList == isBlackList);
    }

    public async Task<Joke> Add(Joke newJoke)
    {
        return await _jokeRepository.Add(newJoke);
    }

    public async Task<Joke> Update(Joke joke)
    {
        var isExist = await Get(joke.Id) != null;
        if (isExist is false)
        {
            return await Add(joke);
        }

        return await _jokeRepository.Update(joke);
    }

    public async Task<Joke?> Like(long jokeId)
    {
        var joke = await Get(jokeId);
        if (joke == null)
        {
            return null;
        }

        var countLike = joke.CountLiked;
        var incrementCountLike = countLike + 1;
        joke.CountLiked = incrementCountLike;
        return await Update(joke);
    }
}