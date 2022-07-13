using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;
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
        /// 用户Id外键
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 父节点（父文件夹）
        /// </summary>
        public Guid? ParentDirInfoId { get; set; }
        /// <summary>
        /// 当前文件夹下的文件列表
        /// </summary>

        public virtual ICollection<UserPrivateFileInfo> ChildrenFileInfo { get; set; }
        /// <summary>
        /// 当前文件夹下的文件夹列表
        /// </summary>
        public virtual ICollection<DirInfo> ChildrenDirInfo { get; set; }
        public virtual DirInfo ParentDirInfo { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual User User { get; set; }

    }
}