using System;
using System.Threading.Tasks;

using Core.Context;
using Core.Data.Entities.Documents;
using Core.Extensions;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface IDocumentManager: IEntityService<DocumentEntity> {
        Task<Pager<DocumentEntity>> FindByCodeAsync(string code);
    }

    public class DocumentManager: AsyncEntityService<DocumentEntity>, IDocumentManager {
        public DocumentManager(IApplicationDbContext context) : base(context) { }

        public Task<Pager<DocumentEntity>> FindByCodeAsync(string code) {
            throw new NotImplementedException();
        }
    }
}
