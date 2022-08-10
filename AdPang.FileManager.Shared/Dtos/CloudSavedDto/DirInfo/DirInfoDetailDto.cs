using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo
{
    public class DirInfoDetailDto : BaseDto<Guid>
    {

        /// <summary>
        /// 文件夹名
        /// </summary>
        public string DirName { get; set; }
        /// <summary>
        /// 当前文件夹下的文件列表
        /// </summary>
        public virtual ICollection<UserPrivateFileInfoDto> ChildrenFileInfo { get; set; } = new List<UserPrivateFileInfoDto>();
        /// <summary>
        /// 当前文件夹下的文件夹列表
        /// </summary>
        public virtual ICollection<DirInfoDetailDto> ChildrenDirInfo { get; set; } = new List<DirInfoDetailDto>();

        /// <summary>
        /// 父节点（父文件夹）
        /// </summary>
        public Guid? ParentDirInfoId { get; set; }



        /*public virtual DirInfoDetailDto ParentDirInfo { get; set; }*/
        ///// <summary>
        ///// 所属用户
        ///// </summary>
        //public virtual UserDto User { get; set; }
    }
}
