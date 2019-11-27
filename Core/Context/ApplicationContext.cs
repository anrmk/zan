using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Context {
    public class ApplicationContext: DbContext, IApplicationContext {
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

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
           // optionsBuilder.UseNpgsql("Host=192.168.0.109;Database=smartwatch;Username=postgres;Password=12345");
        }

        public Task<int> SaveChangesAsync() {
            throw new NotImplementedException();
        }
    }
}
