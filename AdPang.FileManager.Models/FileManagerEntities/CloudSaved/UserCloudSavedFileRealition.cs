using AdPang.FileManager.Models.FileManagerEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    public class UserCloudSavedFileRealition : BaseModel<long>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 云盘文件Id
        /// </summary>
        public Guid CloudSavedFileId { get; set; }
    }
}
