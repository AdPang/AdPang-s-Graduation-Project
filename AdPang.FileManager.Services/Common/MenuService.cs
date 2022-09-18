using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IRepositories.Common;
using AdPang.FileManager.IServices.Common;
using AdPang.FileManager.Models.FileManagerEntities.Common;
using AdPang.FileManager.Services.Base;

namespace AdPang.FileManager.Services.Common
{
    public class MenuService : BaseService<Menu>, IMenuService
    {
        private readonly IMenuRepository menuRepository;

        public MenuService(IMenuRepository  menuRepository) : base(menuRepository)
        {
            this.menuRepository = menuRepository;
        }

        public Task<Menu?> FindMenuDetailAsync(Expression<Func<Menu, bool>> predicate)
        {
            return menuRepository.FindMenuDetailAsync(predicate);
        }
    }
}
