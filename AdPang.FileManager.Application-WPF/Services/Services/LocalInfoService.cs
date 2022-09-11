using System;
using System.Collections.ObjectModel;
using AdPang.FileManager.Application_WPF.Common.Helper;
using AdPang.FileManager.Application_WPF.Common.Models;
using AdPang.FileManager.Application_WPF.Services.IServices;

namespace AdPang.FileManager.Application_WPF.Services.Services
{
    public class LocalInfoService : ILocalInfoService
    {

        public LocalInfoService()
        {
            InitLocalDiskInfos();
        }


        #region 属性

        private ObservableCollection<LocalDiskInfo> localDiskInfos = new();
        public ObservableCollection<LocalDiskInfo> LocalDiskInfos
        {
            get { return localDiskInfos; }
            set { localDiskInfos = value; }
        }
        #endregion
        private void InitLocalDiskInfos()
        {
            string[] drives = Environment.GetLogicalDrives();
            int index = 1;
            foreach (string drive in drives)
            {
                this.LocalDiskInfos.Add(new LocalDiskInfo
                {
                    DriveSN = DiskHelper.GetDiskVolume(drive[0].ToString()),
                    Drive = drive,
                    Id = index++
                });
            }
        }
        public ObservableCollection<LocalDiskInfo> GetLocalDiskInfos()
        {
            return LocalDiskInfos;
        }

    }
}
