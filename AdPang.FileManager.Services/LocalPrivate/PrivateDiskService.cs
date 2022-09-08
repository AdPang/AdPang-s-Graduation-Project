using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Services.Base;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Services.LocalPrivate
{
    public class PrivateDiskService : BaseService<PrivateDiskInfo>, IPrivateDiskService
    {
        private readonly IPrivateDiskRepository privateDiskRepository;

        public PrivateDiskService(IPrivateDiskRepository  privateDiskRepository) : base(privateDiskRepository)
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
