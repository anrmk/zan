using Core.Context;
using Core.Services.Business;
using Core.Services.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Config {
    /// <summary>
    /// Services dependency injection
    /// </summary>
    public class ServiceModuleConfig {
        public static void Configuration(IServiceCollection services) {

            ///Context
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            ///Managers
            services.AddTransient<IUserProfileManager, UserProfileManager>();
            services.AddTransient<IDocumentManager, DocumentManager>();

            ///NSI Managers
            services.AddTransient<INsiLanguageManager, NsiLanguageManager>();
            services.AddTransient<INsiDocumentStatusManager, NsiDocumentStatusManager>();
            services.AddTransient<INsiDocumentTypeManager, NsiDocumentTypeManager>();
            services.AddTransient<INsiRegionManager, NsiRegionManager>();
            services.AddTransient<INsiDevAgencyManager, NsiDevAgencyManager>();
            services.AddTransient<INsiInitRegionManager, NsiInitRegionManager>();
            services.AddTransient<INsiDocumentSectionManager, NsiDocumentSectionManager>();
            services.AddTransient<INsiSourceManager, NsiSourceManager>();
            services.AddTransient<INsiRegAgencyManager, NsiRegAgencyManager>();
            services.AddTransient<INsiClassifierManager, NsiClassifierManager>();
            services.AddTransient<INsiDepartmentManager, NsiDepartmentManager>();
            services.AddTransient<INsiDocumentTitlePrefixManager, NsiDocumentTitlePrefixManager>();
            services.AddTransient<INsiLawForceManager, NsiLawForceManager>();
            services.AddTransient<INsiGrifTypeManager, NsiGrifTypeManager>();

            ///Business
            services.AddTransient<IAccountBusinessService, AccountBusinessService>();
            services.AddTransient<INsiBusinessService, NsiBusinessService>(); //для работы со справочниками
            services.AddTransient<ISyncBusinessService, SyncBusinessService>(); //для работы с синхронизацией данных
        }
    }
}
