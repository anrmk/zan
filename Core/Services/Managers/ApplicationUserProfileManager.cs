using Core.Context;
using Core.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IApplicationUserProfileService: IEntityService<UserProfileEntity> {
    }

    public class ApplicationUserProfileService: AsyncEntityService<UserProfileEntity>, IApplicationUserProfileService {
        public ApplicationUserProfileService(IApplicationDbContext context) : base(context) { }
    }
}
