using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.IRepositories.LocalPrivate
{
    public interface IPrivateFileRepository : IBaseRepository<PrivateFileInfo>
    {
        Task<List<PrivateFileInfo>> GetAllDuplicateAsync(Expression<Func<PrivateFileInfo, bool>> predicate);
    }
}
