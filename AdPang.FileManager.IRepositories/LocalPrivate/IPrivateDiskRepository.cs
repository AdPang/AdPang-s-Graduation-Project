using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.IRepositories.LocalPrivate
{
    public interface IPrivateDiskRepository : IBaseRepository<PrivateDiskInfo>
    {
        Task<List<PrivateDiskInfo>> GetDiskInfoContainFileInfoPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken);
    }
}
