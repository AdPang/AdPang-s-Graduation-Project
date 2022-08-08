using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo
{
    public class UserPrivateFileInfoDetailDto : BaseDto<Guid>
    {
        /// <summary>
        /// 展示给指定用户的文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 当前文件所在文件夹信息
        /// </summary>
        public virtual DirInfoDto CurrentDirectoryInfo { get; set; }
        /// <summary>
        /// 所属用户信息
        /// </summary>
        public virtual UserDto User { get; set; }
        /// <summary>
        /// 真实文件信息
        /// </summary>
        public virtual CloudFileInfoDetailDto RealFileInfo { get; set; }
    }
}
