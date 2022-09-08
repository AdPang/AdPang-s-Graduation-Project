using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.UserPrivateFileInfo;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Common.Events
{
    public class FileTransferMessage
    {
        public string Filter { get; set; }

        public string FilePath { get; set; }
        public Guid CurrentDirId { get; set; }
        public string DownloadDir { get; set; } = string.Empty;


        public UserPrivateFileInfoDto UserPrivateFileInfo { get; set; }

    }
    public class FileTransferEvent : PubSubEvent<FileTransferMessage>
    {


    }
}
