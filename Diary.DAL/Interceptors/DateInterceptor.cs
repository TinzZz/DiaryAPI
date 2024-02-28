using Diary.Domain.Entity;
using Diary.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Interceptors
{
    public class DateInterceptor : SaveChangesInterceptor
    {

        //async
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null)                                  // no changes if dBcontext == null
            {
                return base.SavingChangesAsync(eventData, result);
            }

            var entries = dbContext.ChangeTracker.Entries<IAuditable>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified)
                .ToList();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SavingChangesAsync(eventData, result);
        }

        ////Sync
        //public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        //{
        //    var dbContext = eventData.Context;
        //    if (dbContext == null)                                  // no changes if dBcontext == null
        //    {
        //        return base.SavingChanges(eventData, result);
        //    }

        //    var entries = dbContext.ChangeTracker.Entries<IAuditable>();
        //    foreach ( var entry in entries )
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Property(x => x.CreatedAt).CurrentValue = DateTime.UtcNow;
        //        }

        //        if (entry.State == EntityState.Modified)
        //        {
        //            entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.UtcNow;
        //        }
        //    }

        //    return base.SavingChanges(eventData, result);
        //}
    }
}
