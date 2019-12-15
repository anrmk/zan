using System;
using System.Collections.Generic;
using System.Text;
using Core.Context;
using Core.Data.Entities.Documents;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IDocumentManager: IEntityService<DocumentEntity> {
        //Task<NsiDocumentSectionEntity> FindByCodeAsync(string code);
    }

    public class DocumentManager: AsyncEntityService<DocumentEntity>, IDocumentManager {
        public DocumentManager(IApplicationDbContext context) : base(context) { }

        /*public async Task<NsiDocumentSectionEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }*/
    }
}
