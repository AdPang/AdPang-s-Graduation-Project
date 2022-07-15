using AdPang.FileManager.IRepositories.Base;
using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.IRepositories.CloudSaved
{
    public interface ICloudFileRepository : IBaseRepository<CloudFileInfo>
    {
    }
}
