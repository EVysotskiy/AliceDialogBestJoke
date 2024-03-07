using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Server.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        public override int SaveChanges()
        {
            FillUpdatedDate();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            FillUpdatedDate();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        
        private void FillUpdatedDate()
        {
            var newEntities = ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Added &&
                         x.Entity != null &&
                         x.Entity is ITimeStampedModel
                )
                .Select(x => x.Entity as ITimeStampedModel);

            var modifiedEntities = ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                         x.Entity != null &&
                         x.Entity is ITimeStampedModel
                )
                .Select(x => x.Entity as ITimeStampedModel);

            foreach (var newEntity in newEntities)
            {
                if (newEntity != null)
                {
                    newEntity.CreatedAt = DateTime.UtcNow;
                    newEntity.LastModified = DateTime.UtcNow;
                }
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                if (modifiedEntity != null) modifiedEntity.LastModified = DateTime.UtcNow;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            FillUpdatedDate();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            FillUpdatedDate();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}