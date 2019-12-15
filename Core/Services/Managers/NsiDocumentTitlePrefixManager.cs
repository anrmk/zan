using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface INsiDocumentTitlePrefixManager: IEntityService<NsiDocumentTitlePrefixEntity> {
    }

    public class NsiDocumentTitlePrefixManager: AsyncEntityService<NsiDocumentTitlePrefixEntity>, INsiDocumentTitlePrefixManager {
        public NsiDocumentTitlePrefixManager(IApplicationDbContext context) : base(context) { }

    }
}
