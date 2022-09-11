using System;
using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    public class SharedFileInfo : BaseModel<Guid>
    {
        /// <summary>
        /// 分享文件描述
        /// </summary>
        public string? SharedDesc { get; set; }
        /// <summary>
        /// 分享密码
        /// </summary>
        public string? SharedPassword { get; set; }
        /// <summary>
        /// 分享用户Id外键
        /// </summary>
        public Guid ShardByUserId { get; set; }
        /// <summary>
        /// 分享文件夹外键Id （可以为空）
        /// </summary>
        public Guid? DirId { get; set; }
        /// <summary>
        /// 单个文件信息Id
        /// </summary>
        public Guid? SingleFileId { get; set; }
        /// <summary>
        /// 是否是单个文件
        /// </summary>
        public bool IsSingleFile { get; set; } = false;
        /// <summary>
        /// 是否有过期时间
        /// </summary>
        public bool HasExpired { get; set; } = true;
        /// <summary>
        /// 过期时间（默认7天）
        /// </summary>
        public DateTime? ExpiredTime { get; set; } = DateTime.Now.AddDays(7);
        /// <summary>
        /// 分享文件夹
        /// </summary>
        public virtual DirInfo? DirInfo { get; set; }
        /// <summary>
        /// 分享用户
        /// </summary>
        public virtual User ShardByUser { get; set; }
        /// <summary>
        /// 分享单个文件信息
        /// </summary>
        public virtual UserPrivateFileInfo? SingleFileInfo { get; set; }

    }
}
