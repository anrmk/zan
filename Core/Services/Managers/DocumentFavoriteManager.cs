using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Documents;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface IDocumentFavoriteManager: IEntityService<DocumentFavoriteEntity> {
        Task<List<DocumentFavoriteEntity>> FindListByUserIdAsync(Guid userId);
        Task<DocumentFavoriteEntity> FindByUserIdAsync(Guid userId, Guid documentId);
    }

    public class DocumentFavoriteManager: AsyncEntityService<DocumentFavoriteEntity>, IDocumentFavoriteManager {
        public DocumentFavoriteManager(IApplicationDbContext context) : base(context) { }

        public async Task<DocumentFavoriteEntity> FindByUserIdAsync(Guid userId, Guid documentId) {
            return await DbSet
                .Include(x => x.Document)
                .Include(x => x.ApplicationUser)
                .Where(x => x.ApplicationUserId == userId && x.DocumentId == documentId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<DocumentFavoriteEntity>> FindListByUserIdAsync(Guid userId) {
            return await DbSet
                .Where(x => x.ApplicationUserId == userId).ToListAsync();
        }
    }
}
