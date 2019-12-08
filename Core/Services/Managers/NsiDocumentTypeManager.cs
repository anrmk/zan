using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface INsiDocumentTypeManager: IEntityService<NsiDocumentTypeEntity> {

    }

    public class NsiDocumentTypeManager: AsyncEntityService<NsiDocumentTypeEntity>, INsiDocumentTypeManager {
        public NsiDocumentTypeManager(IApplicationDbContext context) : base(context) { }

    }
}