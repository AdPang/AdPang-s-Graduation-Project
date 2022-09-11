using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo
{
    /// <summary>
    /// 文件分享
    /// </summary>
    public class SharedFileInfoDto : BaseDto<Guid>
    {
        /// <summary>
        /// 分享文件描述
        /// </summary>
        public string? SharedDesc { get; set; }
        /// <summary>
        /// 分享密码
        /// </summary>
        public string? SharedPassword { get; set; }
        /// <summary>
        /// 分享文件夹外键Id （可以为空）
        /// </summary>
        public Guid? DirId { get; set; }
        /// <summary>
        /// 单个文件信息Id
        /// </summary>
        public Guid? SingleFileId { get; set; }
        /// <summary>
        /// 是否是单个文件
        /// </summary>
        public bool IsSingleFile { get; set; } = false;
        /// <summary>
        /// 是否有过期时间
        /// </summary>
        public bool HasExpired { get; set; } = true;
        /// <summary>
        /// 过期时间（默认7天）
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired
        {
            get
            {
                if (HasExpired)
                {
                    return DateTime.Now > ExpiredTime;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
