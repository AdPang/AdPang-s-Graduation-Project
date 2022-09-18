using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class MenuDto : BaseDto<Guid>
    {
        /// <summary>
        /// 图标编号
        /// </summary>
        public string? IconStr { get; set; }
        /// <summary>
        /// 菜单对应路由
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单等级
        /// </summary>
        public int MenuLevel { get; set; }
        /// <summary>
        /// 父菜单Id
        /// </summary>
        public Guid? ParentMenuId { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public virtual ICollection<MenuDto> ChildrenMenu { get; set; } = new List<MenuDto>();
    }
}
