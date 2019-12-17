using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiClassifierManager: IEntityService<NsiClassifierEntity> {
        Task<NsiClassifierEntity> FindByCodeAsync(string code);
    }

    public class NsiClassifierManager: AsyncEntityService<NsiClassifierEntity>, INsiClassifierManager {
        public NsiClassifierManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiClassifierEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
