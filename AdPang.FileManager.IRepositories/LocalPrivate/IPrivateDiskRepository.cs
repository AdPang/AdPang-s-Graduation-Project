using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;

namespace AdPang.FileManager.IRepositories.LocalPrivate
{
    public interface IPrivateDiskRepository : IBaseRepository<PrivateDiskInfo>
    {
        Task<List<PrivateDiskInfo>> GetDiskInfoContainFileInfoPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken);
        Task<List<PrivateDiskInfo>> GetDiskPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken, bool isTracking);
    }
}
