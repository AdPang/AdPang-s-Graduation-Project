using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdPang.FileManager.Application_WPF.Common.Helper
{
    public static class DiskHelper
    {
        #region 获取硬盘序列号
        [DllImport("kernel32.dll")]
        private static extern int GetVolumeInformation(
        string lpRootPathName,
        string lpVolumeNameBuffer,
        int nVolumeNameSize,
        ref int lpVolumeSerialNumber,
        int lpMaximumComponentLength,
        int lpFileSystemFlags,
        string lpFileSystemNameBuffer,
        int nFileSystemNameSize
        );
        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <param name="drvID">硬盘盘符[c|d|e|....]</param>
        /// <returns></returns>
        public static string GetDiskVolume(string drvID)
        {
            const int MAX_FILENAME_LEN = 256;
            int retVal = 0;
            int lpMaximumComponentLength = 0;
            int lpFileSystemFlags = 0;
            string lpVolumeNameBuffer = null;
            string lpFileSystemNameBuffer = null;
            int i = GetVolumeInformation(
            drvID + @":\",
            lpVolumeNameBuffer,
            MAX_FILENAME_LEN,
            ref retVal,
            lpMaximumComponentLength,
            lpFileSystemFlags,
            lpFileSystemNameBuffer,
            MAX_FILENAME_LEN
            );
            return retVal.ToString("x");
        }
        #endregion
    }
}
