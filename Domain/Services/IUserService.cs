using Domain.Model;

namespace Domain.Services;

public interface IUserService
{
    Task<(User user,bool isNewUser)> GetOrCreate(string platformId);
    Task<User> Add(User newUser);
    Task<User> Update(User newUser);
}