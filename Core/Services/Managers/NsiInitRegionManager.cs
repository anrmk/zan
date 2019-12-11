using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiInitRegionManager: IEntityService<NsiInitRegionEntity> {
        Task<NsiInitRegionEntity> FindByCodeAsync(string code);
    }

    public class NsiInitRegionManager: AsyncEntityService<NsiInitRegionEntity>, INsiInitRegionManager {
        public NsiInitRegionManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiInitRegionEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
