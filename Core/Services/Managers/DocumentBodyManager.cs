using System;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Documents;
using Core.Services.Base;

using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IDocumentBodyManager: IEntityService<DocumentBodyEntity> {
        Task<DocumentBodyEntity> FindByDocumentIdAsync(Guid documentId);
    }

    public class DocumentBodyManager: AsyncEntityService<DocumentBodyEntity>, IDocumentBodyManager {
        public DocumentBodyManager(IApplicationDbContext context) : base(context) { }

        public async Task<DocumentBodyEntity> FindByDocumentIdAsync(Guid documentId) {
            return await DbSet.Where(x => x.DocumentId.Equals(documentId)).FirstOrDefaultAsync();
        }
    }
}
