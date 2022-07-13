using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;
using System;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    /// <summary>
    /// 用户操作上传文件信息表
    /// </summary>
    public class UserPrivateFileInfo : BaseModel<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 展示给指定用户的文件名
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 当前所在文件夹的外键Id
        /// </summary>
        public Guid CurrentDirectoryInfoId { get; set; }
        /// <summary>
        /// 当前文件所在文件夹信息
        /// </summary>
        public virtual DirInfo CurrentDirectoryInfo  { get; set; }
        /// <summary>
        /// 所属用户信息
        /// </summary>
        public virtual User User { get; set; }

    }
}
