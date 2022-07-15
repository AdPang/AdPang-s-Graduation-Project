using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.IServices.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Services.CloudSaved
{
    public class CloudFileService : BaseService<CloudFileInfo>, ICloudFileService
    {
        public CloudFileService(IBaseRepository<CloudFileInfo> baseRepository) : base(baseRepository)
        {
        }
    }
}
