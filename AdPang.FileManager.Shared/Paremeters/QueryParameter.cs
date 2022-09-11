namespace AdPang.FileManager.Shared.Paremeters
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryParameter
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string? Search { get; set; }
    }
}
