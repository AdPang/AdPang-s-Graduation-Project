using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.CloudFileInfo;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo
{
    public class UserPrivateFileInfoDto : BaseDto<Guid>
    {
        private string fileName = string.Empty;
        private CloudFileInfoDto? realFileInfo;
        /// <summary>
        /// 展示给指定用户的文件名
        /// </summary>
        public string FileName { get { return fileName; } set { fileName = value; OnPropertyChanged(); } }
        /// <summary>
        /// 真实文件信息
        /// </summary>

        public CloudFileInfoDto? RealFileInfo
        {
            get { return realFileInfo; }
            set { realFileInfo = value; OnPropertyChanged(); }
        }

    }
}
