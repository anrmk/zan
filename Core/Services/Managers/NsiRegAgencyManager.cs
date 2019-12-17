using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiRegAgencyManager: IEntityService<NsiRegAgencyEntity> {
        Task<NsiRegAgencyEntity> FindByCodeAsync(string code);
    }

    public class NsiRegAgencyManager: AsyncEntityService<NsiRegAgencyEntity>, INsiRegAgencyManager {
        public NsiRegAgencyManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiRegAgencyEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
