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
    /// 私有硬盘信息表
    /// </summary>
    public class PrivateDiskInfo : BaseModel<Guid>
    {
        /// <summary>
        /// 硬盘别名
        /// </summary>
        public string DiskName { get; set; } = "暂未命名";
        /// <summary>
        /// 硬盘SN码（唯一标识）
        /// </summary>
        public string DiskSN { get; set; }
        /// <summary>
        /// 所属用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// 私有文件集合
        /// </summary>
        public virtual ICollection<PrivateFileInfo> PrivateFiles { get; set; }
    }
}
