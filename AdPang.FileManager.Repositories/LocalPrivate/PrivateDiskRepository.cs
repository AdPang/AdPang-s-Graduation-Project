using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.Repositories.LocalPrivate
{
    public class PrivateDiskRepository : BaseRepository<PrivateDiskInfo>, IPrivateDiskRepository
    {
        public PrivateDiskRepository(FileManagerDbContext context) : base(context)
        {
        }

        public async Task<List<PrivateDiskInfo>> GetDiskInfoContainFileInfoPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken)
        {
            return await DbContext().PrivateDiskInfos.Take(maxResultCount).Skip(skipCount).OrderBy(sorting).Where(predicate).Include(x => x.User).Include(x => x.PrivateFiles).ToListAsync();
        }

        public async Task<List<PrivateDiskInfo>> GetDiskPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken, bool isTracking)
        {
            return await DbContext().PrivateDiskInfos.Take(maxResultCount).Skip(skipCount).OrderBy(sorting).Where(predicate).Include(x => x.PrivateFiles).ToListAsync();
        }
    }
}
