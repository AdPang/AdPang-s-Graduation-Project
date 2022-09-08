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
        private string diskName = "暂未命名";

        /// <summary>
        /// 硬盘别名
        /// </summary>
        public string DiskName
        {
            get { return diskName; }
            set { diskName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 硬盘SN码（唯一标识）
        /// </summary>
        private string diskSN;

        public string DiskSN
        {
            get { return diskSN; }
            set { diskSN = value;OnPropertyChanged(); }
        }
                   
        private bool canDelete = false;
        /// <summary>
        /// 是否能删除
        /// </summary>
        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; OnPropertyChanged(); }
        }


    }


    
}
