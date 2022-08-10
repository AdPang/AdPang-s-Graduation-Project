using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class UserInfoContainCloudFileInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 所有文件信息
        /// </summary>
        public ICollection<UserPrivateFileInfoDto> UserPrivateFileInfos { get; set; } = new List<UserPrivateFileInfoDto>();
    }
}
