using System.Linq;

namespace ShadyNagy.Utilities.Extensions
{
    public static class DbContextExtensions
    {

#if NETFRAMEWORK
        public static void Reset(this  System.Data.Entity.DbContext context)
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
