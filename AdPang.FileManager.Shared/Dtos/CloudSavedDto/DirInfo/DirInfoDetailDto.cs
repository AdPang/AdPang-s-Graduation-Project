using System.Collections.ObjectModel;
using AdPang.FileManager.Shared.Dtos.Base;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;

namespace AdPang.FileManager.Shared.Dtos.CloudSavedDto.DirInfo
{
    public class DirInfoDetailDto : BaseDto<Guid>
    {
        private ObservableCollection<UserPrivateFileInfoDto> childrenFileInfo = new();
        private ObservableCollection<DirInfoDetailDto> childrenDirInfo = new();
        private Guid? parentDirInfoId;
        private string dirName = string.Empty;
        /// <summary>
        /// 文件夹名
        /// </summary>
        public string DirName { get { return dirName; } set { dirName = value; OnPropertyChanged(); } }
        /// <summary>
        /// 当前文件夹下的文件列表
        /// </summary>
        public ObservableCollection<UserPrivateFileInfoDto> ChildrenFileInfo
        {
            get { return childrenFileInfo; }
            set { childrenFileInfo = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 当前文件夹下的文件夹列表
        /// </summary>
        public ObservableCollection<DirInfoDetailDto> ChildrenDirInfo
        {
            get { return childrenDirInfo; }
            set { childrenDirInfo = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 父节点（父文件夹）
        /// </summary>
        public Guid? ParentDirInfoId { get { return parentDirInfoId; } set { parentDirInfoId = value; OnPropertyChanged(); } }


    }
}
