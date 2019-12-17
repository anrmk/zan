using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiGrifTypeManager: IEntityService<NsiGrifTypeEntity> {
        Task<NsiGrifTypeEntity> FindByCodeAsync(string code);
    }

    public class NsiGrifTypeManager: AsyncEntityService<NsiGrifTypeEntity>, INsiGrifTypeManager {
        public NsiGrifTypeManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiGrifTypeEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
