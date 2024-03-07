using System.Linq.Expressions;
using Domain.Model;
using Server.Database;
using Server.Repositories.Abstraction;

namespace Server.Repositories;

public class JokeRepository: Repository<Joke, long, AppDbContext>
{
    protected virtual Expression<Func<Joke, long>> Key => model => model.Id;

    public JokeRepository(AppDbContext ctx, AppDbContext dbContext) : base(dbContext,
        (appDbContext) => appDbContext.Jokes)
    {

    }
}