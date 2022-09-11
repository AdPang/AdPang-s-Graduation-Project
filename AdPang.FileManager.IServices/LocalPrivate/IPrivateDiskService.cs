﻿using System.Linq.Expressions;
using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;

namespace AdPang.FileManager.IServices.LocalPrivate
{
    public interface IPrivateDiskService : IBaseService<PrivateDiskInfo>
    {
        Task<List<PrivateDiskInfo>> GetDiskInfoContainFileInfoPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default);

        Task<List<PrivateDiskInfo>> GetDiskPagedListAsync(Expression<Func<PrivateDiskInfo, bool>> predicate, int skipCount, int maxResultCount, string sorting, CancellationToken cancellationToken = default, bool IsTracking = false);
    }
}
