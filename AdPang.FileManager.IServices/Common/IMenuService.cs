using System.Linq.Expressions;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.Common;

namespace AdPang.FileManager.IServices.Common
{
    public interface IMenuService : IBaseService<Menu>
    {
        Task<Menu?> FindMenuDetailAsync(Expression<Func<Menu, bool>> predicate);
    }
}
