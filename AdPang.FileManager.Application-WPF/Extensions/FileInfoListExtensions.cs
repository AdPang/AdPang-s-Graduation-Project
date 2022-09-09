using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Events;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Extensions
{
    public static class FileInfoListExtensions
    {
        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterFileInfoMessage(this IEventAggregator aggregator,
            Action<FileInfosMessage> action)
        {
            aggregator.GetEvent<FileInfosEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true);
        }


        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="fileTransferMessage"></param>
        public static void SendFileInfosMessage(this IEventAggregator aggregator, FileInfosMessage fileTransferMessage)
        {
            aggregator.GetEvent<FileInfosEvent>().Publish(fileTransferMessage);
        }
    }
}
