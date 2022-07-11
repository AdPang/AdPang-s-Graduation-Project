using AdPang.FileManager.Models.FileManagerEntities.Base;
using System;
using System.Collections.Generic;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    /// <summary>
    /// 文件夹
    /// </summary>
    public class DirInfo : BaseModel<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 父节点（父文件夹）
        /// </summary>
        public Guid? ParentDirectoryInfoId { get; set; }
        /// <summary>
        /// 当前文件夹下的文件列表
        /// </summary>

        public virtual ICollection<UserPrivateFileInfo> ChildrenFileInfo { get; set; } = new List<UserPrivateFileInfo>();
        /// <summary>
        /// 当前文件夹下的文件夹列表
        /// </summary>
        public virtual ICollection<DirInfo> ChildrenDirectoryInfo { get; set; } = new List<DirInfo>();

    }
}