using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class UserInfoContainDirInfoDto : BaseDto<Guid>
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
        /// 文件夹信息
        /// </summary>
        public ICollection<DirInfoDetailDto> DirInfos { get; set; } = new List<DirInfoDetailDto>();
    }
}
