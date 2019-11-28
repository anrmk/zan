using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IUserProfileManager: IEntityService<UserProfileEntity> {
    }

    public class UserProfileManager: AsyncEntityService<UserProfileEntity>, IUserProfileManager {
        public UserProfileManager(IApplicationDbContext context) : base(context) { }
    }
}
