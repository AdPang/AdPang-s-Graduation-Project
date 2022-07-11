using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.FileManagerEntities.LocalPrivate
{
    /// <summary>
    /// 私有文件信息表
    /// </summary>
    public class PrivateFileInfo : BaseModel<Guid>, IBaseFiles
    {
        /// <summary>
        /// 私有硬盘外键
        /// </summary>
        public Guid DiskInfoId { get; set; }
        /// <summary>
        /// 私有硬盘
        /// </summary>
        public virtual PrivateDiskInfo DiskInfo { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string? FileType { get; set; }
        public long FileLength { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}
