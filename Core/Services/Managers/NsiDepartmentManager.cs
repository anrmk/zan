using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Data.Entities.Nsi;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Managers {
    public interface INsiDepartmentManager: IEntityService<NsiDepartmentEntity> {
        Task<NsiDepartmentEntity> FindByCodeAsync(string code);
    }

    public class NsiDepartmentManager: AsyncEntityService<NsiDepartmentEntity>, INsiDepartmentManager {
        public NsiDepartmentManager(IApplicationDbContext context) : base(context) { }

        public async Task<NsiDepartmentEntity> FindByCodeAsync(string code) {
            return await DbSet.Where(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
