using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdPang.FileManager.Application_WPF.Common.Events;
using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Extensions
{
    public static class FileSharedExtensions
    {
        /// <summary>
        /// 注册提示消息 
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="action"></param>
        public static void ResgiterFileSharedMessage(this IEventAggregator aggregator,
            Action<SharedFileInfoDetailDto> action)
        {
            aggregator.GetEvent<FileSharedEvent>().Subscribe(action,
                ThreadOption.PublisherThread, true);
        }


        /// <summary>
        /// 发送提示消息
        /// </summary>
        /// <param name="aggregator"></param>
        /// <param name="message"></param>
        public static void SendFileSharedMessage(this IEventAggregator aggregator, SharedFileInfoDetailDto  sharedFileInfoDetailDto)
        {
            aggregator.GetEvent<FileSharedEvent>().Publish(sharedFileInfoDetailDto);
        }
    }
}
