using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo
{
    public class DirInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string DirName { get; set; }
        /// <summary>
        /// 父节点（父文件夹）
        /// </summary>
        public Guid? ParentDirInfoId { get; set; }
    }
}
