using System;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Core.Context {
    public class ApplicationDbContext: IdentityDbContext<ApplicationUserEntity>, IApplicationDbContext {
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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public Task<int> SaveChangesAsync() {
            throw new NotImplementedException();
        }
    }
}
