using Hitmu.Abstractions.Store.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Hitmu.Providers.EfCore
{
    public abstract class EfDataContext : DbContext, IDataContext
    {
        protected EfDataContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}