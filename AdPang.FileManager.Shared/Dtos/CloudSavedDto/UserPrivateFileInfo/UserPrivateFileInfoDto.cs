using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo
{
    public class UserPrivateFileInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 展示给指定用户的文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 真实文件信息
        /// </summary>
        public virtual CloudFileInfoDto RealFileInfo { get; set; }
    }
}
