using System;
using System.Collections.Generic;
using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    /// <summary>
    /// 云盘文件信息表
    /// </summary>
    public class CloudFileInfo : BaseModel<Guid>, IBaseFiles
    {
        /// <summary>
        /// 真实文件名（Guid打乱）
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 真实文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }
        /// <summary>
        /// 用户外键Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 用户数量
        /// </summary>
        public long UserCount { get; set; } = 0;
        /// <summary>
        /// 上传用户
        /// </summary>
        public virtual User UploadBy { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
