using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class UserDetailDto : BaseDto<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string? PhoneNumber { get; set; }
        public string? RolesStr { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool? LockoutEnabled { get; set; }
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
}
