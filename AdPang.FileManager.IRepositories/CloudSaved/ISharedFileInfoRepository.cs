using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.IRepositories.CloudSaved
{
    public interface ISharedFileInfoRepository : IBaseRepository<SharedFileInfo>
    {
        Task<List<SharedFileInfo>> GetMySharedInfoListAsync(Expression<Func<SharedFileInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken, bool isTracking);
    }
}
