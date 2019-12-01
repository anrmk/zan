using Core.Context;
using Core.Services.Business;
using Core.Services.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Config {
    /// <summary>
    /// Services dependency injection
    /// </summary>
    public class ServiceModuleConfig {
        public static void Configuration(IServiceCollection services) {
            ///Context
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            

            ///Managers
            services.AddTransient<IUserProfileManager, UserProfileManager>();

            ///Business
            services.AddTransient<IAccountBusinessService, AccountBusinessService>();
        }
    }
}
