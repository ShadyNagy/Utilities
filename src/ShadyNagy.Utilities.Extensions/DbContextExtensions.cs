using System.Linq;

namespace ShadyNagy.Utilities.Extensions
{
    public static class DbContextExtensions
    {

#if NETFRAMEWORK
        public static void Reset(this System.Data.Entity.DbContext context)
        {
            var entries = context.ChangeTracker
                .Entries()
                .Where(e => e.State != System.Data.Entity.EntityState.Unchanged)
                .ToArray();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case System.Data.Entity.EntityState.Modified:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                    case System.Data.Entity.EntityState.Added:
                        entry.State = System.Data.Entity.EntityState.Detached;
                        break;
                    case System.Data.Entity.EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }

        public static string[] GetKeyNames<TEntity>(this System.Data.Entity.DbContext context)
            where TEntity : class
        {
            return context.GetKeyNames(typeof(TEntity));
        }
 
        public static string[] GetKeyNames(this System.Data.Entity.DbContext context, System.Type entityType)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;
 
            // Get the mapping between CLR types and metadata OSpace
            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));
 
            // Get metadata for given CLR type
            var entityMetadata = metadata
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(e => objectItemCollection.GetClrType(e) == entityType);
 
            return entityMetadata.KeyProperties.Select(p => p.Name).ToArray();
        }

#else
        public static void Reset(this Microsoft.EntityFrameworkCore.DbContext context)
        {
            var entries = context.ChangeTracker
                .Entries()
                .Where(e => e.State != Microsoft.EntityFrameworkCore.EntityState.Unchanged)
                .ToArray();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case Microsoft.EntityFrameworkCore.EntityState.Modified:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        break;
                    case Microsoft.EntityFrameworkCore.EntityState.Added:
                        entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                        break;
                    case Microsoft.EntityFrameworkCore.EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
#endif
    }
}
