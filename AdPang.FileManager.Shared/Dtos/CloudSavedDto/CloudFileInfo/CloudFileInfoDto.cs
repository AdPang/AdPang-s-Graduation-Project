using AdPang.FileManager.Shared.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo
{
    public class CloudFileInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 真实文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }
    }
}
