using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiRegionManager: IEntityService<NsiRegionEntity> {
        Task<IEnumerable<NsiRegionEntity>> FindByParentId(Guid parentId);
    }

    public class NsiRegionManager: AsyncEntityService<NsiRegionEntity>, INsiRegionManager {
        public NsiRegionManager(IApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<NsiRegionEntity>> FindByParentId(Guid parentId) {
            return await DbSet.Where(x => x.ParentId == parentId).ToListAsync();
        }
    }
}