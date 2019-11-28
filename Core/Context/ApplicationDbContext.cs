using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Data.Entities;
using Core.Data.Entities.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Core.Context {
    public class ApplicationDbContext: DbContext, IApplicationDbContext {
        /**
         * Первоначальное создание базы. Из консоли исполнить команды:
         * 1. Enable-Migrations
         * 2. Add-Migration InitialMigration
         * 2. Update-Database
         * Последующие миграции:
         * Аналогично, главное не затирать старые миграции и заново не включать миграции
         * Версия миграции: 1.0.1
         */
        public Database ApplicationDatabase { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync() {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach(var entry in modifiedEntries) {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if(entity != null) {
                    string identityName = ""; // Thread.CurrentPrincipal.Identity.Name;
                    DateTime now = DateTime.UtcNow;

                    if(entry.State == EntityState.Added) {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    } else {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }
                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }
            bool saveFailed;
            do {
                saveFailed = false;
                try {
                    return await base.SaveChangesAsync();
                } catch(DbUpdateConcurrencyException) {
                    saveFailed = true;
                    return -1001;
                } catch(DbUpdateException) {
                    saveFailed = true;
                    return -1002;
                } catch(Exception e) {
                    Console.WriteLine(e.Message);
                    saveFailed = true;
                    return -1003;
                }
            } while(saveFailed);
        }

        #region ENTITIES
        public DbSet<ApplicationUserEntity> ApplicationUsers { get; set; }
        public DbSet<UserProfileEntity> UserProfiles { get; set; }
        #endregion
    }
}
