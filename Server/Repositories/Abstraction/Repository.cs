using System.Linq.Expressions;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Server.Repositories.Abstraction
{
    public abstract class Repository<TModel, TKey, TDbContext> where TModel : class, ITimeStampedModel
        where TDbContext : DbContext
    {
        private readonly DbSet<TModel> _dBSet;
        private readonly TDbContext _dbContext;

        protected Repository(TDbContext dbContext, Func<TDbContext, DbSet<TModel>> dBSet)
        {
            _dbContext = dbContext;
            _dBSet = dBSet(dbContext);
        }
        
        public async Task<TModel?> First(Expression<Func<TModel, bool>> predicate)
        {
            return await _dBSet.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TModel> Add(TModel model)
        {
            try
            {
                await _dBSet.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException exception)
            {
                return null;
            }
        }
        
        public async Task<TModel> Update(TModel entity)
        {
            _dBSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TModel[]> Where(Expression<Func<TModel, bool>> predicate)
        {
            return await _dBSet.Where(predicate).AsNoTracking().ToArrayAsync();
        }

        public async Task<TModel[]> Where(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TKey>> sortPredicate)
        {
            return await _dBSet.Where(predicate).AsNoTracking().OrderByDescending(sortPredicate).ToArrayAsync();
        }
        
        public async Task<TModel[]> Where(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TKey>> sortPredicate,int take)
        {
            return await _dBSet.Where(predicate).AsNoTracking().OrderByDescending(sortPredicate).Take(take).ToArrayAsync();
        }
    }
}