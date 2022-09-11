using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;

namespace AdPang.FileManager.IRepositories.CloudSaved
{
    public interface IDirInfoRepository : IBaseRepository<DirInfo>
    {
        //Task<DirInfo> GetRootDirInfosAsync(Guid userId);
        Task<IList<DirInfo>> GetDirDetailListAsync(Expression<Func<DirInfo, bool>> predicate);
    }
}
