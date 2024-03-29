﻿using AdPang.FileManager.IServices.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.IServices.CloudSaved
{
    public interface IDirService : IBaseService<DirInfo>
    {
        //Task<DirInfo> GetRootDirInfosAsync(Guid userId);
        Task<IList<DirInfo>> GetDirDetailListAsync(Expression<Func<DirInfo, bool>> predicate);
    }
}
