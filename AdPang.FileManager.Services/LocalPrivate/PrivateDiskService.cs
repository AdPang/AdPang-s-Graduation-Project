using System.Linq.Expressions;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Services.Base;

namespace AdPang.FileManager.Services.LocalPrivate
{
    public class PrivateDiskService : BaseService<PrivateDiskInfo>, IPrivateDiskService
    {
        private readonly IPrivateDiskRepository privateDiskRepository;

        public PrivateDiskService(IPrivateDiskRepository privateDiskRepository) : base(privateDiskRepository)
        {
            this.privateDiskRepository = privateDiskRepository;
        }

        public async Task<List<PrivateDiskInfo>> GetDiskInfoContainFileInfoPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default)
        {
            return await privateDiskRepository.GetDiskInfoContainFileInfoPagedListAsync(predicate, skipCount, maxResultCount, sorting, cancellationToken);
        }

        public async Task<List<PrivateDiskInfo>> GetDiskPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default, bool IsTracking = false)
        {
            return await privateDiskRepository.GetDiskPagedListAsync(predicate, skipCount, maxResultCount, sorting, cancellationToken, IsTracking);
        }
    }
}
