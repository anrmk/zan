using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiDocumentSectionManager: IEntityService<NsiDocumentSectionEntity> {
        Task<NsiDocumentSectionEntity> FindByCodeAsync(string code);
    }

    public class NsiDocumentSectionManager: AsyncEntityService<NsiDocumentSectionEntity>, INsiDocumentSectionManager {
        public NsiDocumentSectionManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiDocumentSectionEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
