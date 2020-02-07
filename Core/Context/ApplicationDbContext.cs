using System;
using System.Linq;
using System.Threading.Tasks;

using Core.Data.Entities;
using Core.Data.Entities.Base;
using Core.Data.Entities.Documents;
using Core.Data.Entities.Nsi;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Context {
    //IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
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
        //private IHostingEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly string _contentRootPath = "";

        public Database ApplicationDatabase { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, IHostingEnvironment environment) : base(options) {
            _configuration = configuration;
            _contentRootPath = environment.ContentRootPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            string conn = _configuration.GetConnectionString("DefaultConnection");
            //var dir = AppDomain.CurrentDomain.BaseDirectory;
            //if(conn.Contains("%CONTENTROOTPATH%")) {
            //    conn = conn.Replace("%CONTENTROOTPATH%", _contentRootPath);
            //}
            optionsBuilder.UseSqlServer(conn);
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<NsiLanguageEntity>().Property(e => e.Id).ValueGeneratedNever();
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
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<DocumentBodyEntity> DocumentBodies { get; set; }
        public DbSet<DocumentFavoriteEntity> DocumentFavorites { get; set; }
        #endregion

        #region NSI
        public DbSet<NsiLanguageEntity> NsiLanguages { get; set; }
        public DbSet<NsiDocumentStatusEntity> NsiDocumentStatuses { get; set; }
        public DbSet<NsiDocumentTypeEntity> NsiDocumentTypes { get; set; }
        public DbSet<NsiRegionEntity> NsiRegions { get; set; }
        public DbSet<NsiDevAgencyEntity> NsiDevAgencies { get; set; }
        public DbSet<NsiInitRegionEntity> NsiInitRegions { get; set; }
        public DbSet<NsiDocumentSectionEntity> NsiDocSections { get; set; }
        public DbSet<NsiSourceEntity> NsiSources { get; set; }
        public DbSet<NsiRegAgencyEntity> NsiRegAgencies { get; set; }
        public DbSet<NsiClassifierEntity> NsiClassifiers { get; set; }
        public DbSet<NsiDepartmentEntity> NsiDepartments { get; set; }
        public DbSet<NsiDocumentTitlePrefixEntity> NsiDocTitlePrefixes { get; set; }
        public DbSet<NsiLawForceEntity> NsiLawForces { get; set; }
        public DbSet<NsiGrifTypeEntity> nsiGrifTypes { get; set; }
        #endregion
    }
}
