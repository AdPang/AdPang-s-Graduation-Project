using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;
using AdPang.FileManager.Models.FileManagerEntities.Common;

namespace AdPang.FileManager.Common.Extensions
{
    /// <summary>
    /// List拓展
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="dirInfos"></param>
        public static void Merge(this IEnumerable<DirInfo> dirInfos, DirInfo parent)
        {
            var children = dirInfos.Where(x => x.ParentDirInfoId == parent.Id);
            foreach (var child in children)
            {
                parent.ChildrenDirInfo.Add(child);
                dirInfos.Merge(child);
            }
        }
        /// <summary>
        /// 菜单合并
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="parent"></param>
        public static void Merge(this IEnumerable<Menu> menus,Menu parent)
        {
            var children = menus.Where(x => x.ParentMenuId == parent.Id);
            foreach (var child in children)
            {
                parent.ChildrenMenu.Add(child);
                menus.Merge(child);
            }
        }
        /// <summary>
        /// 找父菜单
        /// </summary>
        /// <param name="childrenMenu"></param>
        /// <param name="allMenus">所有菜单集合</param>
        /// <param name="result">结果</param>
        public static void FindParentMenu(this Menu childrenMenu,IEnumerable<Menu> allMenus,List<Menu> result)
        {
            if(!result.Contains(childrenMenu))
                result.Add(childrenMenu);
            if (childrenMenu.ParentMenuId == null) 
                return;
            var parentMenu = allMenus.Where(x => x.Id.Equals(childrenMenu.ParentMenuId)).FirstOrDefault();
            if (parentMenu != null)
                FindParentMenu(parentMenu, allMenus, result);
        }
    }
}
