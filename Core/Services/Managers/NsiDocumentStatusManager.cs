using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiDocumentStatusManager: IEntityService<NsiDocumentStatusEntity> {
        Task<NsiDocumentStatusEntity> FindByCodeAsync(string code);
        Task<IEnumerable<NsiDocumentStatusEntity>> FindByCodesAsync(string[] code);
    }

    public class NsiDocumentStatusManager: AsyncEntityService<NsiDocumentStatusEntity>, INsiDocumentStatusManager {
        public NsiDocumentStatusManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiDocumentStatusEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NsiDocumentStatusEntity>> FindByCodesAsync(string[] code) {
            return await DbSet.Where(x => code.Contains(x.Code)).ToListAsync();
        }
    }
}
