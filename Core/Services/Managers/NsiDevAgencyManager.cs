using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiDevAgencyManager: IEntityService<NsiDevAgencyEntity> {
        Task<NsiDevAgencyEntity> FindByCodeAsync(string code);
    }

    public class NsiDevAgencyManager: AsyncEntityService<NsiDevAgencyEntity>, INsiDevAgencyManager {
        public NsiDevAgencyManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiDevAgencyEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
