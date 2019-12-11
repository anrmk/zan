using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiDocSectionManager: IEntityService<NsiDocSectionEntity> {
        Task<NsiDocSectionEntity> FindByCodeAsync(string code);
    }

    public class NsiDocSectionManager: AsyncEntityService<NsiDocSectionEntity>, INsiDocSectionManager {
        public NsiDocSectionManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiDocSectionEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
