using Domain.Model;
using Domain.Services;
using Logic.Extension;
using Microsoft.Extensions.Caching.Memory;

namespace Server.Services;

public class CachedUserService : IUserService
{
    private readonly IUserService _userService;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _ttl = new(600000000);
    private readonly ILogger<UserService> _logger;
    
    private const string KeyPrefix = "User";


    public CachedUserService(ILogger<UserService> logger, IUserService userService, IMemoryCache cache)
    {
        _logger = logger;
        _userService = userService;
        _cache = cache;
    }
    
    public async Task<(User,bool)> GetOrCreate(string platformId)
    {
        return await _cache.Remember($"{KeyPrefix}:{platformId}",
            async () => await _userService.GetOrCreate(platformId), _ttl);
    }

    public async Task<User> Add(User newUser)
    {
        return await _userService.Add(newUser);
    }
    
    public async Task<User> Update(User newUser)
    {
        return await _userService.Update(newUser);
    }
}