namespace AdPang.FileManager.Shared.Paremeters
{
    public class PrivateFileInfoQueryParameter : QueryParameter
    {
        public string? DiskId { get; set; }
        public RequestMode RequestMode { get; set; } = RequestMode.Default;
    }
    /// <summary>
    /// 请求数据模式
    /// </summary>
    public enum RequestMode
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,
        /// <summary>
        /// 去重
        /// </summary>
        Distinct = 1,

    }
}
