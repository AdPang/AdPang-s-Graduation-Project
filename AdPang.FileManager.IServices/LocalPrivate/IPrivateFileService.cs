using System.Linq.Expressions;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;

namespace AdPang.FileManager.IServices.LocalPrivate
{
    public interface IPrivateFileService : IBaseService<PrivateFileInfo>
    {
        Task<List<PrivateFileInfo>> GetAllDuplicateAsync(Expression<Func<PrivateFileInfo, bool>> predicate);
    }
}
