using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;

namespace AdPang.FileManager.IRepositories.LocalPrivate
{
    public interface IPrivateFileRepository : IBaseRepository<PrivateFileInfo>
    {
        Task<List<PrivateFileInfo>> GetAllDuplicateAsync(Expression<Func<PrivateFileInfo, bool>> predicate);
    }
}
