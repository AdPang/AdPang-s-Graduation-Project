using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IServices.LocalPrivate;
using AdPang.FileManager.Models.FileManagerEntities.LocalPrivate;
using AdPang.FileManager.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Services.LocalPrivate
{
    public class PrivateDiskService : BaseService<PrivateDiskInfo>, IPrivateDiskService
    {
        public PrivateDiskService(IBaseRepository<PrivateDiskInfo> baseRepository) : base(baseRepository)
        {
        }
    }
}
