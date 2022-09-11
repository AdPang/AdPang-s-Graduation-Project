namespace AdPang.FileManager.Models.FileManagerEntities.Base
{
    public interface IBaseFiles
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMD5Str { get; set; }
    }
}
