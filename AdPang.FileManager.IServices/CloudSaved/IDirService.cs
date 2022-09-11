using System.Linq.Expressions;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;

namespace AdPang.FileManager.IServices.CloudSaved
{
    public interface IDirService : IBaseService<DirInfo>
    {
        //Task<DirInfo> GetRootDirInfosAsync(Guid userId);
        Task<IList<DirInfo>> GetDirDetailListAsync(Expression<Func<DirInfo, bool>> predicate);
    }
}
