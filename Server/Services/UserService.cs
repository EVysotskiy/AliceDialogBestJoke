using Domain.Model;
using Domain.Services;
using Server.Repositories;

namespace Server.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(UserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    private async Task<User?> Get(long id)
    {
        return await _userRepository.First(x => x.Id == id);
    }

    private async Task<User?> Get(string platformId)
    {
        return await _userRepository.First(x => x.PlatformId == platformId);
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platformId"></param>
    /// <param name="isNewUser"></param>
    /// <returns></returns>
    public async Task<(User,bool)> GetOrCreate(string platformId)
    {
        var isNewUser = false;
        var user = await Get(platformId);
        if (user == null)
        {
            user = await Add(new User(platformId));
            isNewUser = true;
        }

        return (user, isNewUser);
    }

    public async Task<User> Add(User newUser)
    {
        return await _userRepository.Add(newUser);
    }

    public async Task<User> Update(User newUser)
    {
        var foundUser = await _userRepository.First(x => x.Id == newUser.Id);
        if (foundUser == null)
        {
            return await _userRepository.Add(newUser);
        }
        
        return await _userRepository.Update(newUser);
    }
}