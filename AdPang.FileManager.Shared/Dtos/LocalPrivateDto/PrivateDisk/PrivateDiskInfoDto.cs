using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.LocalPrivateDto.PrivateDisk
{
    public class PrivateDiskInfoDto : BaseDto<Guid>
    {
        private string diskName = "";

        /// <summary>
        /// 硬盘别名
        /// </summary>
        public string DiskName
        {
            get { return diskName; }
            set { diskName = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 计算机名称
        /// </summary>
        private string? computerName;
        public string? ComputerName
        {
            get { return computerName; }
            set { computerName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 硬盘盘符
        /// </summary>
        private string? driveName;

        public string? DriveName
        {
            get { return driveName; }
            set { driveName = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 硬盘SN码（唯一标识）
        /// </summary>
        private string diskSN;

        public string DiskSN
        {
            get { return diskSN; }
            set { diskSN = value; OnPropertyChanged(); }
        }

        private bool canDelete = false;
        /// <summary>
        /// 是否能删除
        /// </summary>
        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; OnPropertyChanged(); }
        }


    }



}
