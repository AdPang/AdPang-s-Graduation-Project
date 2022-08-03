using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk
{
    public class PrivateDiskInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 硬盘别名
        /// </summary>
        public string DiskName { get; set; } = "暂未命名";
        /// <summary>
        /// 硬盘SN码（唯一标识）
        /// </summary>
        public string DiskSN { get; set; }
    }


    
}
