using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface INsiRegionManager: IEntityService<NsiRegionEntity> {
    }

    public class NsiRegionManager: AsyncEntityService<NsiRegionEntity>, INsiRegionManager {
        public NsiRegionManager(IApplicationDbContext context) : base(context) { }
      
    }
}
