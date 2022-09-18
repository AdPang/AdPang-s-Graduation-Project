using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Models.FileManagerEntities.Base;
using AdPang.FileManager.Models.IdentityEntities;

namespace AdPang.FileManager.Models.FileManagerEntities.Common
{
    public class Menu : BaseModel<Guid>
    {
        /// <summary>
        /// 图标编号
        /// </summary>
        public string? IconStr { get; set; } = string.Empty;
        /// <summary>
        /// 菜单对应路由
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 父菜单Id
        /// </summary>
        public Guid? ParentMenuId { get; set; }
        /// <summary>
        /// 菜单等级
        /// </summary>
        public int MenuLevel { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        /// <summary>
        /// 父菜单详情
        /// </summary>
        public virtual Menu? ParentMenu { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public virtual ICollection<Menu> ChildrenMenu { get; set; } = new List<Menu>();

    }
}
