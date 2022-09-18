using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.Common;

namespace AdPang.FileManager.IRepositories.Common
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<Menu?> FindMenuDetailAsync(Expression<Func<Menu, bool>> predicate);
    }
}
