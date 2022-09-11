using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class UserInfoContainDiskInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        ///// <summary>
        ///// 电话
        ///// </summary>
        //public string PhoneNumber { get; set; }
        public ICollection<DiskInfoContainFileInfoDto> PrivateDiskInfos { get; set; } = new List<DiskInfoContainFileInfoDto>();
    }
}
