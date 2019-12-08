using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiLanguageManager: IEntityService<NsiLanguageEntity> {
        Task<NsiLanguageEntity> FindByCodeAsync(string code);
    }

    public class NsiLanguageManager: AsyncEntityService<NsiLanguageEntity>, INsiLanguageManager {
        public NsiLanguageManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiLanguageEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
