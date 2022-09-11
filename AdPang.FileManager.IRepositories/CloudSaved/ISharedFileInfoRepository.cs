using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;

namespace AdPang.FileManager.IRepositories.CloudSaved
{
    public interface ISharedFileInfoRepository : IBaseRepository<SharedFileInfo>
    {
        Task<List<SharedFileInfo>> GetMySharedInfoListAsync(Expression<Func<SharedFileInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken, bool isTracking);
    }
}
