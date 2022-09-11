using AdPang.FileManager.Models.FileManagerEntities.CloudSaved;

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
    }
}
