using System.Linq.Expressions;
using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.Common;
using AdPang.FileManager.Models.FileManagerEntities.Common;
using AdPang.FileManager.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.Repositories.Common
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        private readonly FileManagerDbContext context;

        public MenuRepository(FileManagerDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Menu?> FindMenuDetailAsync(Expression<Func<Menu, bool>> predicate)
        {
            return await context.Menus.Include(x => x.Roles).Where(predicate).FirstOrDefaultAsync();
        }
    }
}
