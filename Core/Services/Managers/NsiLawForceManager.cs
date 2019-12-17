using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiLawForceManager: IEntityService<NsiLawForceEntity> {
        Task<NsiLawForceEntity> FindByCodeAsync(string code);
    }

    public class NsiLawForceManager: AsyncEntityService<NsiLawForceEntity>, INsiLawForceManager {
        public NsiLawForceManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiLawForceEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
