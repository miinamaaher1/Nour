using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XCourse.Core.Entities;

namespace XCourse.Infrastructure.Data.Interceptors
{
    public class AppUserInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplySoftDelete(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplySoftDelete(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplySoftDelete(DbContext? context)
        {
            if (context == null)
            {
                return;
            }

            foreach (var entry in context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted && e.Entity is AppUser))
            {
                entry.State = EntityState.Modified;
                entry.CurrentValues["IsDeleted"] = true;
                entry.CurrentValues["DeleteDate"] = DateTime.Now;
            }
        }
    }
}
