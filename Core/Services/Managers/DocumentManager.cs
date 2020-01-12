using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Documents;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IDocumentManager: IEntityService<DocumentEntity> {
        Task<DocumentEntity> FindInclude(Guid id);
        Task<List<DocumentEntity>> FindAllByNgrAsync(string ngr);
        Task<DocumentEntity> FindByNgrAsync(string ngr, int lng, DateTime editionDate);
    }

    public class DocumentManager: AsyncEntityService<DocumentEntity>, IDocumentManager {
        public DocumentManager(IApplicationDbContext context) : base(context) { }

        public async Task<DocumentEntity> FindInclude(Guid id) {
            return await DbSet
               .Include(x => x.Language)
               .Include(x => x.RegionAction)
               .Include(x => x.Status)
               .Include(x => x.Section)
               .Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<List<DocumentEntity>> FindAllByNgrAsync(string ngr) {
            return await DbSet
                .Include(x => x.Language)
                .Where(x => x.Ngr == ngr).ToListAsync();
        }

        public async Task<DocumentEntity> FindByNgrAsync(string ngr, int lng, DateTime editionDate) {
            return await DbSet
                .Where(x => x.Ngr == ngr)
                .Where(x => x.LanguageId == lng)
                .Where(x => x.EditionDate == editionDate)
                .SingleOrDefaultAsync();
        }
    }
}
