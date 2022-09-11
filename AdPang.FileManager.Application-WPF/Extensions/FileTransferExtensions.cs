using System;
using AdPang.FileManager.Application_WPF.Common.Events;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Extensions
{
    public static class FileTransferExtensions
    {
        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterFileTransferMessage(this IEventAggregator aggregator,
            Action<FileTransferMessage> action, string filterName = "Download")
        {
            aggregator.GetEvent<FileTransferEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true, (m) =>
                {
                    return m.Filter.Equals(filterName);
                });
        }


        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendFileTransferMessage(this IEventAggregator aggregator, FileTransferMessage fileTransferMessage, string filterName = "Download")
        {
            fileTransferMessage.Filter = filterName;
            aggregator.GetEvent<FileTransferEvent>().Publish(fileTransferMessage);
        }
    }
}
