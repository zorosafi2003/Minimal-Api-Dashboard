using Microsoft.EntityFrameworkCore;
using MiniApp.Dal.Entities;
using MiniApp.Dal.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniApp.Dal.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Project { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<TimeSheet> TimeSheet { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.UpdateSoftDeleteStates();

            var count = await base.SaveChangesAsync();

            return count;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetQueryFilter(modelBuilder);

        }

        private void UpdateSoftDeleteStates()
        {
            if (ChangeTracker.Entries() != null)
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    if ((entry.Entity is EntityBase) || (entry.Entity is IEntityBase))
                    {
                        var domainEntity = (entry.Entity is EntityBase) ? (EntityBase)entry.Entity : (IEntityBase)entry.Entity;

                        if (entry.State == EntityState.Added)
                        {
                            if (domainEntity.CreateDate == null)
                            {
                                domainEntity.CreateDate = DateTime.Now;
                            }
                         
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            domainEntity.UpdatedDate = DateTime.Now;
                        }

                    }
                }
            }
        }

        #region Global Query For Multi Tenant

        private void SetQueryFilter(ModelBuilder builder)
        {
            foreach (var type in GetEntityTypes())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type);
                method.Invoke(this, new object[] { builder });
            }
        }

        static readonly MethodInfo SetGlobalQueryMethod = typeof(ApplicationContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                        .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            builder.Entity<T>().HasQueryFilter(e => e.DeletedOn == null);
        }

        private static IList<Type> _entityTypeCache;
        private static IList<Type> GetEntityTypes()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }

            var baseEntityType = typeof(EntityBase);
            var types = baseEntityType.Assembly.GetTypes();
            _entityTypeCache = types.Where(x => !x.IsAbstract && baseEntityType.IsAssignableFrom(x)).ToList();

            return _entityTypeCache;
        }

        #endregion
    }
}
