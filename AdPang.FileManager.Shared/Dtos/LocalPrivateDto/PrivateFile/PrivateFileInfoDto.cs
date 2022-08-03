using AdPang.FileManager.Shared.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile
{
    public class PrivateFileInfoDto:BaseDto<Guid>
    {
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }
        /// <summary>
        /// 硬盘Id
        /// </summary>
        public Guid DiskId { get; set; }
    }
}
