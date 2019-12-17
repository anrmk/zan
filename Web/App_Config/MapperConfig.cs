using AutoMapper;

using Core.Data.Dto;
using Core.Data.Entities;

using Microsoft.Extensions.DependencyInjection;

namespace Web.App_Config {
    public class MapperConfig: Profile {
        public static void Register(IServiceCollection services) {
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new Core.Config.MapperConfig());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
        }

        private MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>();
            CreateMap<UserProfileEntity, UserProfileDto>();
        }
    }
}
