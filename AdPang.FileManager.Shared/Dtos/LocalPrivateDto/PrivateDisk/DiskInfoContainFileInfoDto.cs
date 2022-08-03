using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateFile;
using AdPang.FileManager.Shared.Dtos.SystemCommon;

namespace AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk
{
    public class DiskInfoContainFileInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 硬盘别名
        /// </summary>
        public string DiskName { get; set; } = "暂未命名";
        /// <summary>
        /// 硬盘SN码（唯一标识）
        /// </summary>
        public string DiskSN { get; set; }

        public UserDto User { get; set; }

        public ICollection<PrivateFileInfoDto> PrivateFiles { get; set; } = new List<PrivateFileInfoDto>();
    }
}
