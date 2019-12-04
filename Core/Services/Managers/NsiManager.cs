using Core.Context;
using Core.Data.Entities;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface INsiManager: IEntityService<NsiEntity> {
    }

    public class NsiManager: AsyncEntityService<NsiEntity>, INsiManager {
        public NsiManager(IApplicationDbContext context) : base(context) { }
    }
}
