using Core.Context;
using Core.Repositories;
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

            ///Business
            services.AddTransient<IAccountBusinessService, AccountBusinessService>();
        }
    }
}
