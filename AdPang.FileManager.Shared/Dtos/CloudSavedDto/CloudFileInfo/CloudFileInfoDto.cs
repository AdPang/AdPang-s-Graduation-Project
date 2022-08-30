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
        private string? fileType = string.Empty;
        private string fileMD5Str = string.Empty;
        private DateTime updateTime;
        private long fileLength;
        /// <summary>
        /// 真实文件类型
        /// </summary>
        public string? FileType { get { return fileType; } set { fileType = value; OnPropertyChanged(); } }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get { return fileLength; } set { fileLength = value;OnPropertyChanged(); } }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get { return fileMD5Str; } set { fileMD5Str = value; OnPropertyChanged(); } }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value;OnPropertyChanged(); }
        }

    }
}
