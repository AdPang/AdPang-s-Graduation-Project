using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo
{
    public class SharedFileInfoDetailDto : BaseDto<Guid>
    {
        /// <summary>
        /// 分享文件描述
        /// </summary>
        private string? sharedDesc;

        public string? SharedDesc
        {
            get { return sharedDesc; }
            set { sharedDesc = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 分享密码
        /// </summary>
        private string? sharedPassword;

        public string? SharedPassword
        {
            get { return sharedPassword; }
            set { sharedPassword = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 分享文件夹外键Id （可以为空）
        /// </summary>
        private Guid? dirId;

        public Guid? DirId
        {
            get { return dirId; }
            set { dirId = value;OnPropertyChanged(); }
        }
        /// <summary>
        /// 单个文件信息Id
        /// </summary>
        private Guid? singleFileId;

        public Guid? SingleFileId
        {
            get { return singleFileId; }
            set { singleFileId = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// 是否是单个文件
        /// </summary>
        private bool isSingleFile = false;

        public bool IsSingleFile
        {
            get { return isSingleFile; }
            set { isSingleFile = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 是否有过期时间
        /// </summary>
        private bool hasExpired = true;

        public bool HasExpired
        {
            get { return hasExpired; }
            set { hasExpired = value;OnPropertyChanged(); }
        }
        /// <summary>
        /// 过期时间（默认7天）
        /// </summary>
        private DateTime? expiredTime;

        public DateTime? ExpiredTime
        {
            get { return expiredTime; }
            set { expiredTime = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// 分享文件夹
        /// </summary>
        private DirInfoDetailDto? dirInfo;

        public DirInfoDetailDto? DirInfo
        {
            get { return dirInfo; }
            set { dirInfo = value;OnPropertyChanged(); }
        }

        /// <summary>
        /// 分享单个文件信息
        /// </summary>
        private UserPrivateFileInfoDto? singleFileInfo;

        public UserPrivateFileInfoDto? SingleFileInfo
        {
            get { return singleFileInfo; }
            set { singleFileInfo = value;OnPropertyChanged(); }
        }

    }
}
