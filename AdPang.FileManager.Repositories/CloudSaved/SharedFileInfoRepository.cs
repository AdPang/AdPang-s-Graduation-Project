using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace AdPang.FileManager.Repositories.CloudSaved
{
    public class SharedFileInfoRepository : BaseRepository<SharedFileInfo>, ISharedFileInfoRepository
    {
        public SharedFileInfoRepository(FileManagerDbContext context) : base(context)
        {
        }

        public async Task<List<SharedFileInfo>> GetMySharedInfoListAsync(Expression<Func<SharedFileInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken, bool isTracking)
        {
            return await DbContext().SharedFileInfos.Where(predicate).OrderBy(sorting).Take(maxResultCount).Skip(skipCount).Include(x => x.DirInfo).Include(x => x.SingleFileInfo).ToListAsync(cancellationToken);
        }
    }
}
