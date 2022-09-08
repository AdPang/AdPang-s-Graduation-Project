using System.Linq.Expressions;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;

namespace AdPang.FileManager.IServices.CloudSaved
{
    public interface ISharedFileInfoService : IBaseService<SharedFileInfo>
    {
        Task<List<SharedFileInfo>> GetMySharedInfoListAsync(Expression<Func<SharedFileInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default, bool IsTracking = false);
    }
}
