using System;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Common.Events
{
    public class FileInfosMessage
    {
        public Guid DiskId { get; set; }
    }


    public class FileInfosEvent : PubSubEvent<FileInfosMessage>
    {


    }
}
