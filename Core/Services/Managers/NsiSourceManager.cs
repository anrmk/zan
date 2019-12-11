using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiSourceManager: IEntityService<NsiSourceEntity> {
        Task<NsiSourceEntity> FindByCodeAsync(string code);
    }

    public class NsiSourceManager: AsyncEntityService<NsiSourceEntity>, INsiSourceManager {
        public NsiSourceManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiSourceEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
