using System;
using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;

namespace AdPang.FileManager.Models.FileManagerEntities.LocalPrivate
{
    /// <summary>
    /// 私有文件信息表
    /// </summary>
    public class PrivateFileInfo : BaseModel<Guid>, IBaseFiles
    {

        /// <summary>
        /// 用户Id外键
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 私有硬盘外键
        /// </summary>
        public Guid DiskId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// 私有硬盘
        /// </summary>
        public virtual PrivateDiskInfo DiskInfo { get; set; }
    }
}
