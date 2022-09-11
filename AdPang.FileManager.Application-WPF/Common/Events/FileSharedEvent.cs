using AdPang.FileManager.Shared.Dtos.CloudSavedDto.SharedFileInfo;
using Prism.Events;

namespace AdPang.FileManager.Application_WPF.Common.Events
{
    internal class FileSharedEvent : PubSubEvent<SharedFileInfoDetailDto>
    {
    }
}
