using AdPang.FileManager.Models.FileManagerEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Models.FileManagerEntities.CloudSaved
{
    public class CloudFileInfo : BaseModel<Guid>, IBaseFiles
    {
        /// <summary>
        /// 真实文件名（Guid打乱）
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        /// <summary>
        /// 真实文件类型
        /// </summary>
        public string? FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileLength { get; set; }
        /// <summary>
        /// 文件存储路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
        /// <summary>
        /// 上传用户Id
        /// </summary>
        public Guid UpdateBy { get; set; }
    }
}
