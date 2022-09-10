using AdPang.FileManager.EntityFrameworkCore.FileManagerDb;
using AdPang.FileManager.IRepositories.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Repositories.LocalPrivate
{
    public class PrivateFileRepository : BaseRepository<PrivateFileInfo>, IPrivateFileRepository
    {
        public PrivateFileRepository(FileManagerDbContext context) : base(context)
        {
        }

        public async Task<List<PrivateFileInfo>> GetAllDuplicateAsync(Expression<Func<PrivateFileInfo, bool>> predicate)
        {
            var fileInfos = await DbContext().PrivateFileInfos.Where(predicate).ToListAsync();
            var groupMd5Str = fileInfos.GroupBy(x => x.FileMD5Str);
            List<PrivateFileInfo> result = new();
            foreach (var item in groupMd5Str)
            {
                if(item.Count() >= 2)
                {
                    result.AddRange(item);
                }
            }
            return result;
        }
    }
}
