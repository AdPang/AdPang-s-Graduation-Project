using AdPang.FileManager.Models.FileManagerEntities.Base;
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
        /// 
        /// </summary>
        public Guid CurrentDirectoryInfoId { get; set; }

        public virtual DirInfo CurrentDirectoryInfo  { get; set; }

    }
}
