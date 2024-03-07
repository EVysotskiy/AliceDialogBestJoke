using System.Linq.Expressions;
using Domain.Model;
using Server.Database;
using Server.Repositories.Abstraction;

namespace Server.Repositories;

public class UserRepository: Repository<User, long, AppDbContext>
{
    protected virtual Expression<Func<User, long>> Key => model => model.Id;

    public UserRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext,
        (appDbContext) => appDbContext.Users)
    {

    }
}