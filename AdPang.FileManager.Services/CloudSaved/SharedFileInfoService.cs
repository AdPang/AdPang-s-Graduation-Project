using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IRepositories.CloudSaved;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Services.CloudSaved
{
    public class SharedFileInfoService : BaseService<SharedFileInfo>, ISharedFileInfoService
    {
        private readonly ISharedFileInfoRepository sharedFileInfoRepository;

        public SharedFileInfoService(ISharedFileInfoRepository sharedFileInfoRepository) : base(sharedFileInfoRepository)
        {
            this.sharedFileInfoRepository = sharedFileInfoRepository;
        }

        public async Task<List<SharedFileInfo>> GetMySharedInfoListAsync(Expression<Func<SharedFileInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await sharedFileInfoRepository.GetMySharedInfoListAsync(predicate, skipCount, maxResultCount, sorting, cancellationToken, IsTracking);
        }
    }
}
