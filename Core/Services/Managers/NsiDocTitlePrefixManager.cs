using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;

namespace Core.Services.Managers {
    public interface INsiDocTitlePrefixManager: IEntityService<NsiDocTitlePrefixEntity> {
    }

    public class NsiDocTitlePrefixManager: AsyncEntityService<NsiDocTitlePrefixEntity>, INsiDocTitlePrefixManager {
        public NsiDocTitlePrefixManager(IApplicationDbContext context) : base(context) { }

    }
}
