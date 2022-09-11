using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.SystemCommon;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo
{
    public class CloudFileInfoDetailDto : BaseDto<Guid>
    {
        /// <summary>
        /// 真实文件名（Guid打乱）
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 真实文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }

        public virtual ICollection<UserDto> Users { get; set; } = new List<UserDto>();
    }
}
